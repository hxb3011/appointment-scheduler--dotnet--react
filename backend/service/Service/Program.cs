using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;

namespace AppointmentScheduler.Service;

public static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;
        services.AddInfrastructure(jwtConfigure: builder.Configuration.GetSection("JWTSettings").ConfigureJSONWebToken);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();
        services.AddRouting(ConfigureRoute);
        var app = builder.Build();

        app.UseInfrastructure();

        app.UseHttpsRedirection();
        app.UseStatusCodePages();

        app.MapControllers();

        app.Run();

        return 0;
    }

    private static void ConfigureJSONWebToken(this IConfiguration configuration, JSONWebTokenOptions options)
    {
        var key = configuration["SymmetricSecurityKey"];
        if (key != null) options.SymmetricSecurityKey = key;
    }

    private static void ConfigureRoute(RouteOptions options)
    {
        options.LowercaseUrls = true;
    }
}