using System.Linq.Expressions;
using AppointmentScheduler.Domain;
using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Entities;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Authorization;
using AppointmentScheduler.Infrastructure.Business;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppointmentScheduler.Infrastructure;

public static class Extensions
{
    public static IEnumerable<T> Cached<T>(this IEnumerable<T> query) => new CachedEnumerable<T>(query);

    public static T WaitForResult<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        if (!task.IsCompleted) task.Wait(timeout, cancellationToken);
        return task.Result;
    }

    public static T WaitForResult<T>(this Task<T> task, int millisecondsTimeout = Timeout.Infinite, CancellationToken cancellationToken = default)
    {
        if (!task.IsCompleted) task.Wait(millisecondsTimeout, cancellationToken);
        return task.Result;
    }

    private static unsafe uint NewId()
    {
        var guid = Guid.NewGuid();
        uint* p = (uint*)&guid;
        return p[0] ^ p[1] ^ p[2] ^ p[3];
    }

    public static TDelegate Method<TDelegate>(this object entity, string methodName)
        where TDelegate : Delegate
        => (TDelegate)Delegate.CreateDelegate(typeof(TDelegate), entity, methodName);

    public static Func<T> Getter<T>(this object entity, string idPropertyName)
        => entity.Method<Func<T>>($"get_{idPropertyName}");

    public static Action<T> Setter<T>(this object entity, string idPropertyName)
        => entity.Method<Action<T>>($"set_{idPropertyName}");

    public static async Task<bool> IdGenerated<TEntity>(this DbContext context, TEntity entity, string idPropertyName) where TEntity : class
    {
        var src = Expression.Parameter(typeof(TEntity));
        var dst = Expression.Constant(entity);
        var query = context.Set<TEntity>().Where(Expression.Lambda<Func<TEntity, bool>>(
            Expression.Equal(
                Expression.Property(src, idPropertyName),
                Expression.Property(dst, idPropertyName)
            ), src
        ));

        const long IdLimit = uint.MaxValue + 1L;
        Task<long> countTask = null;
        var setId = entity.Setter<uint>(idPropertyName);
        do setId(NewId());
        while (await query.AnyAsync() && (
            (countTask ??= context.Set<TEntity>().LongCountAsync())
                .IsCompleted ? countTask.Result : await countTask
        ) <= IdLimit);
        return countTask == null || countTask.Result <= IdLimit;
    }

    public static async Task<TEntity> Initialize<TEntity>(this IRepository repository, TEntity entity)
        where TEntity : class, IBehavioralEntity
        => entity is not IRepositoryEntityInitializer initializer
            || await initializer.Initialize(repository) ? entity : null;

    private static bool InvertTaskResultContinuation(Task<bool> task) => !task.Result;
    public static Task<bool> InvertTaskResult(this Task<bool> task) => task.ContinueWith(InvertTaskResultContinuation);

    private static TOptions Factory<TOptions>(
        this Action<IServiceProvider, TOptions> configure,
        IServiceProvider provider) where TOptions : class, new()
    {
        var options = new TOptions();
        configure?.Invoke(provider, options);
        return options;
    }

    private static void ConfigureFormOptions(IServiceProvider provider, FormOptions options)
        => options.MultipartBodyLengthLimit = 1L << 30;

    private static IServiceCollection AddConfigurator<TService>(
        this IServiceCollection services, Action<IServiceProvider, TService> configurator,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) where TService : class, new()
    {
        services.Add(new ServiceDescriptor(typeof(TService), configurator.Factory, lifetime));
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbConfigure = null,
        Action<IServiceProvider, JSONWebTokenOptions> jwtConfigure = null,
        Action<IServiceProvider, FormOptions> formConfigure = null,
        Action<IServiceProvider, PasswordHasherOptions> passwordHasherConfigure = null
    ) => services.AddDbContext<IRepository, DefaultRepository>(dbConfigure, ServiceLifetime.Singleton)
        .AddConfigurator(jwtConfigure, ServiceLifetime.Singleton)
        .AddConfigurator(ConfigureFormOptions + formConfigure, ServiceLifetime.Singleton)
        .AddConfigurator(passwordHasherConfigure, ServiceLifetime.Singleton)
        .AddSingleton<IPasswordHasher<IUser>, PasswordHasher<IUser>>();

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        => app.UseMiddleware<JSONWebTokenMiddleware>();

    public static async Task Preload(this IRepository repository, IPasswordHasher<IUser> passwordHasher, ILogger logger = null)
    {
        string PreloadKey = "config.preloader.state";
        string PreloadPrefix = typeof(Extensions).Namespace + ".Preloader";
        string PreloadSuccess = PreloadPrefix + ".Success";
        string PreloadAbort = PreloadPrefix + ".Abort";

        var db = await repository.GetService<DbContext>();
        using var transaction = db.Database.BeginTransaction();
        var configs = await repository.GetService<IConfigurationPropertiesService>();
        var preload = configs.GetProperty(PreloadKey, PreloadAbort);

        if (!PreloadSuccess.Equals(preload))
        {
            var admin_role = await repository.ObtainEntity<IRole>();
            admin_role.Name = "Doctor Administrator Role";
            admin_role.Description = "Created by Preloader";
            Enum.GetValues<Permission>().GrantTo(admin_role);
            await admin_role.Create();

            var doctor_role = await repository.ObtainEntity<IRole>();
            doctor_role.Name = "Doctor";
            doctor_role.Description = "Created by Preloader";
            Enum.GetValues<Permission>().Exclude(
                Permission.SystemPrivilege,
                Permission.ReadRole,
                Permission.CreateRole,
                Permission.UpdateRole,
                Permission.DeleteRole,
                Permission.DeleteUser,
                Permission.CreateDiagnosticService,
                Permission.UpdateDiagnosticService,
                Permission.DeleteDiagnosticService
            ).GrantTo(doctor_role);
            await doctor_role.Create();

            var user_role = await repository.ObtainEntity<IRole>();
            user_role.Name = "User";
            user_role.Description = "Created by Preloader";
            new Permission[] {
                    Permission.ReadUser,
                    Permission.UpdateUser,
                    Permission.ReadProfile,
                    Permission.CreateProfile,
                    Permission.UpdateProfile,
                    Permission.DeleteProfile,
                    Permission.ReadDiagnosticService,
                    Permission.ReadAppointment,
                    Permission.CreateAppointment,
                    Permission.DeleteAppointment,
                    Permission.ReadExamination,
                    Permission.ReadPrescription
                }.GrantTo(user_role);
            await user_role.Create();

            if (repository.TryGetKeyOf(user_role, out string role_id))
                configs.SetProperty(RoleImpl.DefaultRoleKey, role_id);

            var admin_user = await repository.ObtainEntity<IDoctor>();
            admin_user.UserName = "root00";
            admin_user.Password = passwordHasher.HashPassword(admin_user, "HeLlo|12");
            admin_user.FullName = "Nguyễn Văn A";
            admin_user.Email = "admin@scheduler-appointment.localhost";
            admin_user.Phone = "0987654321";
            admin_user.Position = "";
            admin_user.Certificate = "";
            await admin_user.ChangeRole(admin_role);
            await admin_user.Create();

            configs.SetProperty(PreloadKey, PreloadSuccess);
        }
        transaction.Commit();
    }
}