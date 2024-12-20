﻿using AppointmentScheduler.Domain;
using AppointmentScheduler.Infrastructure;
using AppointmentScheduler.Infrastructure.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace AppointmentScheduler.Service;

public static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var services = builder.Services;
        // services.AddSwaggerGen();

        services.AddCors(new CorsPolicy() { Origins = { "*" }, Methods = { "*" }, Headers = { "*" } }.AsDefaultPolicyToAdd);
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

        app.UseHttpsRedirection();
        app.UseStatusCodePages();

        app.MapControllers();

        app.UseCors();
        app.UseInfrastructure();
        app.Run();

        return 0;
    }

    private static void AsDefaultPolicyToAdd(this CorsPolicy policy, CorsOptions options)
        => options.AddDefaultPolicy(policy);

    private static void ConfigureDbContext(IServiceProvider provider, DbContextOptionsBuilder optionsBuilder)
    {
        string database, server, user, password;
        if (string.IsNullOrWhiteSpace(database = "DB_DATABASE".Env())) database = "apomtschedsys";
        if (string.IsNullOrWhiteSpace(password = "DB_PASSWORD".Env())) password = "HeLlo|12";
        if (!uint.TryParse("DB_PORT".Env(), out uint port)) port = 3306;
        if (string.IsNullOrWhiteSpace(server = "DB_SERVER".Env())) server = "localhost";
        if (string.IsNullOrWhiteSpace(user = "DB_USERNAME".Env())) user = "user0";
        optionsBuilder.UseMySQL(new MySqlConnectionStringBuilder
        {
            Database = database,
            Password = password,
            Port = port,
            Server = server,
            UserID = user,
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