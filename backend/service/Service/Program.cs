using AppointmentScheduler.Domain;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Microsoft.OpenApi.Models;

namespace AppointmentScheduler.Service;

public static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        // Thêm Swagger services
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Appointment Scheduler API",
                Version = "v1",
                Description = "API Documentation for Appointment Scheduler"
            });
        });

        services.AddInfrastructure
        (
            dbConfigure: ConfigureDbContext,
            jwtConfigure: builder.Configuration.GetSection("JWTSettings").ConfigureJSONWebToken
        );

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddProblemDetails();
        services.AddRouting(ConfigureRoute);

        var app = builder.Build();

        // Cấu hình Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Appointment Scheduler API v1");
                c.RoutePrefix = string.Empty; // Mặc định mở Swagger UI ở root (/)
            });
        }

        app.UseInfrastructure();
        app.UseHttpsRedirection();
        app.UseStatusCodePages();
        app.MapControllers();

        app.Run();

        return 0;
    }

    private static void ConfigureDbContext(IServiceProvider provider, DbContextOptionsBuilder optionsBuilder)
    {
        string database, server, user, password;
        if (string.IsNullOrWhiteSpace(database = "DB_DATABASE".Env())) database = "apomtschedsys";
        if (string.IsNullOrWhiteSpace(password = "DB_PASSWORD".Env())) password = "root";
        if (string.IsNullOrWhiteSpace(server = "DB_SERVER".Env())) server = "localhost";
        if (string.IsNullOrWhiteSpace(user = "DB_USERNAME".Env())) user = "root";
        optionsBuilder.UseMySQL(new MySqlConnectionStringBuilder
        {
            Database = "apomtschedsys",
            Password = "root",
            Port = 3306,
            Server = "localhost",
            UserID = "root",
        }.ConnectionString);
    }

    private static void ConfigureJSONWebToken(this IConfiguration configuration, IServiceProvider provider, JSONWebTokenOptions options)
    {
        var key = configuration["SymmetricSecurityKey"];
        if (key != null) options.SymmetricSecurityKey = key;
    }

    private static void ConfigureRoute(RouteOptions options)
    {
        options.LowercaseUrls = true;
    }
}
