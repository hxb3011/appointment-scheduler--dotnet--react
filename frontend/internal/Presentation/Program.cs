

using AppointmentScheduler.Domain;
using AppointmentScheduler.Presentation.Services.AppointmentService;
using Services.IService;
using Services.Service;

namespace AppointmentScheduler.Presentation;

public static class Program
{
    public static int Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddHttpClient("api", ConfigureApiHttpClient);

		var httpClientBaseAddress = builder.Configuration.GetSection("HttpClientSettings:BaseAddress").Value;



		builder.Services.AddSingleton<IUserService, UserService>();
        builder.Services.AddSingleton<IPatientService, PatientService>();


        builder.Services.AddHttpClient<IAppointmentService, AppointmentService>(client =>
        {
            client.BaseAddress = new Uri(httpClientBaseAddress);
        });

		builder.Services.AddControllersWithViews();

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
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
        return 0;
    }

    private static void ConfigureApiHttpClient(IServiceProvider provider, HttpClient client)
    {
        string? host;
        if (string.IsNullOrWhiteSpace(host = "API_SERVER".Env())) host = "localhost";
        if (!int.TryParse("API_PORT".Env(), out int portNumber)) portNumber = 8080;
        client.BaseAddress = new UriBuilder("https", host, portNumber).Uri;
    }
}

