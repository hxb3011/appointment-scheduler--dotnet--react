using AppointmentScheduler.Domain.Business;
using AppointmentScheduler.Infrastructure.Authorization;
using AppointmentScheduler.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentScheduler.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<JSONWebTokenOptions>? jwtConfigure = null)
    {
        services.AddDbContext<IRepository, DefaultRepository>(ServiceLifetime.Singleton);
        services.AddSingleton<JSONWebTokenOptions>();
        if (jwtConfigure != null) services.Configure(jwtConfigure);
        return services;
    }
    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseMiddleware<JSONWebTokenMiddleware>();
        return app;
    }
}
