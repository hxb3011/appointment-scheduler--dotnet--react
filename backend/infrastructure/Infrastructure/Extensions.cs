using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Authorization;
using AppointmentScheduler.Infrastructure.Business;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

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

    internal static async Task<bool> IdGeneratedWrap<TEntity>(this DbContext context, IQueryable<TEntity> query, TEntity entity, string idPropertyName) where TEntity : class
    {
        const long IdLimit = uint.MaxValue + 1L;

        // Sử dụng reflection để lấy setter của Id
        var propertyInfo = typeof(TEntity).GetProperty(idPropertyName);
        if (propertyInfo == null || propertyInfo.PropertyType != typeof(uint))
        {
            throw new ArgumentException("The specified property is not of type uint.");
        }

        // Tạo ra một task đếm số bản ghi hiện tại trong database
        Task<long> countTask = context.Set<TEntity>().LongCountAsync();

        // Lặp cho đến khi tìm được Id hợp lệ
        while (await query.AnyAsync() && (await countTask) <= IdLimit)
        {
            var newId = NewId();
            propertyInfo.SetValue(entity, newId);
        }

        // Trả về true nếu số bản ghi trong database không vượt quá giới hạn Id
        return (await countTask) <= IdLimit;
    }


    public static async Task<TEntity> Initialize<TEntity>(this IRepository repository, TEntity entity)
        where TEntity : class, IBehavioralEntity
        => entity is not IRepositoryEntityInitializer initializer
            || await initializer.Initialize(repository) ? entity : null;

    private static bool InvertTaskResultContinuation(Task<bool> task) => !task.Result;
    public static Task<bool> InvertTaskResult(this Task<bool> task) => task.ContinueWith(InvertTaskResultContinuation);

    private static JSONWebTokenOptions Factory(this Action<IServiceProvider, JSONWebTokenOptions> configure, IServiceProvider provider)
    {
        var options = new JSONWebTokenOptions();
        configure?.Invoke(provider, options);
        return options;
    }

    private static FormOptions Factory(this Action<IServiceProvider, FormOptions> configure, IServiceProvider provider)
    {
        var options = new FormOptions() { MultipartBodyLengthLimit = 1L << 30 };
        configure?.Invoke(provider, options);
        return options;
    }

    private static PasswordHasherOptions Factory(this Action<IServiceProvider, PasswordHasherOptions> configure, IServiceProvider provider)
    {
        if (configure == null) return null;
        var options = new PasswordHasherOptions();
        configure(provider, options);
        return options;
    }

    private static IServiceCollection AddDescriptor<TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory, ServiceLifetime lifetime) where TService : class
    {
        services.Add(new ServiceDescriptor(typeof(TService), factory, lifetime));
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbConfigure = null,
        Action<IServiceProvider, JSONWebTokenOptions> jwtConfigure = null,
        Action<IServiceProvider, FormOptions> formConfigure = null,
        Action<IServiceProvider, PasswordHasherOptions> passwordHasherConfigure = null
    ) => services.AddDbContext<IRepository, DefaultRepository>(dbConfigure, ServiceLifetime.Singleton)
        .AddDescriptor(jwtConfigure.Factory, ServiceLifetime.Singleton)
        .AddDescriptor(formConfigure.Factory, ServiceLifetime.Singleton)
        .AddDescriptor(passwordHasherConfigure.Factory, ServiceLifetime.Singleton);

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        => app.UseMiddleware<JSONWebTokenMiddleware>();
}