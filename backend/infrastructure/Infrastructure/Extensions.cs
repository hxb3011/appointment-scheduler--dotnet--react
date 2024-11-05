using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Authorization;
using AppointmentScheduler.Infrastructure.Business;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
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

    private static JSONWebTokenOptions Factory(this Action<IServiceProvider, JSONWebTokenOptions> configure, IServiceProvider provider)
    {
        var options = new JSONWebTokenOptions();
        configure?.Invoke(provider, options);
        return options;
    }

    private static IServiceCollection AddDescriptor<TService>(this IServiceCollection services, Func<IServiceProvider, TService> factory, ServiceLifetime lifetime) where TService : class
    {
        services.Add(new ServiceDescriptor(typeof(JSONWebTokenOptions), factory, lifetime));
        return services;
    }

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        Action<IServiceProvider, DbContextOptionsBuilder> dbConfigure = null,
        Action<IServiceProvider, JSONWebTokenOptions> jwtConfigure = null
    ) => services.AddDbContext<IRepository, DefaultRepository>(dbConfigure, ServiceLifetime.Singleton)
        .AddDescriptor(jwtConfigure.Factory, ServiceLifetime.Singleton);

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
        => app.UseMiddleware<JSONWebTokenMiddleware>();
}
