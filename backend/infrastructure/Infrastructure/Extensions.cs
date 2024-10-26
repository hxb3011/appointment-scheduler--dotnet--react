using AppointmentScheduler.Domain.Repositories;
using AppointmentScheduler.Infrastructure.Authorization;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentScheduler.Infrastructure;

public static class Extensions
{
    private static unsafe int NewId()
    {
        var guid = Guid.NewGuid();
        int* p = (int*)&guid;
        return p[0] ^ p[1] ^ p[2] ^ p[3];
    }

    internal static async Task<bool> IdGeneratedWrap<TEntity>(this DbContext context, IQueryable<TEntity> query, TEntity entity, string idPropertyName) where TEntity : class
    {
        const long IdLimit = uint.MaxValue + 1L;
        Task<long> countTask = null;
        var setId = (Action<int>)Delegate.CreateDelegate(typeof(Action<int>), entity, $"set_{idPropertyName}");
        do setId(NewId());
        while (await query.AnyAsync() && (
            (countTask ??= context.Set<TEntity>().LongCountAsync())
                .IsCompleted ? countTask.Result : await countTask
        ) <= IdLimit);
        return countTask == null || countTask.Result <= IdLimit;
    }

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
