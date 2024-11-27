using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using AppointmentScheduler.Domain;
using AppointmentScheduler.Presentation.Attributes;
using AppointmentScheduler.Presentation.Services;

namespace AppointmentScheduler.Presentation;

public static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        services.AddHttpClient();
        services.AddConfigurator<HttpClient>(ConfigureApiHttpClient, ServiceLifetime.Singleton);
        services.AddConfigurator<JsonSerializerOptions>(ConfigureJSONSerializerOptions, ServiceLifetime.Singleton);
        // services.AddApiHttpClientServices(builder.Configuration);
        services.AddApiHttpClientServices();
        services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Apply the CORS policy
        app.UseCors("AllowSpecificOrigin");

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Dashboard}/{action=Index}/{id?}");

        app.Run();
        return 0;
    }

    [Obsolete]
    private static void ConfigureApiHttpClient(
        this IConfiguration configuration, IServiceProvider provider, HttpClient client)
        => client.BaseAddress = new Uri(configuration["BaseAddress"]);

    [Obsolete]
    private static IServiceCollection AddApiHttpClientServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        Action<IServiceProvider, HttpClient> configureClient
            = configuration.GetSection("HttpClientSettings").ConfigureApiHttpClient;
        services.AddHttpClient<AppointmentService>("api", configureClient);
        services.AddHttpClient<DoctorService>("api", configureClient);
        services.AddHttpClient<ProfileService>("api", configureClient);
        services.AddHttpClient<DiagnosticService>("api", configureClient);
        return services;
    }

    private static IServiceCollection AddApiHttpClientServices(this IServiceCollection services)
    {
        services.AddScoped<HttpApiService>();
        services.AddScoped<AppointmentService>();
        services.AddScoped<DoctorService>();
        services.AddScoped<ProfileService>();
        services.AddScoped<DiagnosticService>();
        return services;
    }

    private static TOptions Factory<TOptions>(
        this Action<IServiceProvider, TOptions> configure,
        IServiceProvider provider) where TOptions : class, new()
    {
        var options = new TOptions();
        configure?.Invoke(provider, options);
        return options;
    }

    private static IServiceCollection AddServiceDescriptor(
        this IServiceCollection services, Type serviceType,
        Func<IServiceProvider, object> factory,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        services.Add(new ServiceDescriptor(serviceType, factory, lifetime));
        return services;
    }

    private static IServiceCollection AddServiceDescriptor<TService>(
        this IServiceCollection services, Func<IServiceProvider, TService> factory,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) where TService : class, new()
        => services.AddServiceDescriptor(typeof(TService), factory, lifetime);

    private static IServiceCollection AddConfigurator<TService>(
        this IServiceCollection services, Action<IServiceProvider, TService> configurator,
        ServiceLifetime lifetime = ServiceLifetime.Scoped) where TService : class, new()
        => services.AddServiceDescriptor(configurator.Factory, lifetime);

    private static void ConfigureApiHttpClient(IServiceProvider provider, HttpClient client)
    {
        string host;
        if (string.IsNullOrWhiteSpace(host = "API_SERVER".Env())) host = "localhost";
        if (!int.TryParse("API_PORT".Env(), out int portNumber)) portNumber = 8080;
        client.BaseAddress = new UriBuilder("https", host, portNumber).Uri;
    }

    private static void ConfigureJSONSerializerOptions(IServiceProvider provider,
        JsonSerializerOptions options) => options.LoadDeafult();

    public static dynamic GetMetadata<T>(this T value) where T : struct, Enum
    {
        IDictionary<string, object> props = new Dictionary<string, object>();
        var type = typeof(T);
        var attrs =
            (!Enum.IsDefined(value) ? (MemberInfo)type : type.GetField(Enum.GetName(value),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .GetCustomAttributes<MetadataAttribute>().FirstOrDefault((MetadataAttribute)null);
        string key = "";
        foreach (var v in attrs.Value)
        {
            var isProp = Regex.IsMatch(v, @"^[A-Za-z_][A-Za-z0-9_]*$");
            if (!props.TryGetValue(key, out var o) || o is not StringBuilder builder)
            {
                if (!isProp) continue;
            }
            else if (!isProp || builder.Length == 0)
            {
                builder.Append(v);
                continue;
            }
            if (!props.TryGetValue(key = v, out o) || o is not StringBuilder)
                props[key] = new StringBuilder();
        }
        IDictionary<string, object> metadata = new ExpandoObject();
        foreach (var v in props.Keys)
        {
            key = props[v].ToString();
            StringBuilder builder = new();
            int start = 0;
            foreach (Match match in Regex.Matches(key, @"\{(name|value(:(2|8|10|16))?|)\}", RegexOptions.IgnoreCase))
            {
                var selector = match.Groups[1].Value;
                builder.Append(key, start, match.Index - start);
                start = match.Index + match.Length;

                if ("".Equals(selector)) builder.Append(value.ToString());
                else if ("name".Equals(selector)) builder.Append(Enum.GetName(value));
                else if ("value".Equals(selector) || "value".Equals(selector))
                    builder.Append(Convert.ChangeType(value, Enum.GetUnderlyingType(typeof(T))).ToString());
                else builder.Append("10".Equals(selector = match.Groups[2].Value)
                    ? Convert.ChangeType(value, Enum.GetUnderlyingType(typeof(T))).ToString()
                    : Convert.ToString(Convert.ToInt64(value), "8".Equals(selector) ? 8 : "2".Equals(selector) ? 2 : 16));
            }
            builder.Append(key, start, key.Length - start);
            metadata[v] = builder.ToString();
        }
        return metadata;
    }
}

