

using System.Dynamic;
using System.Reflection;
using System.Text;
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

        // Add services to the container
        builder.Services.AddHttpClient("api", ConfigureApiHttpClient);

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

    public static dynamic GetMetadata<T>(this T value) where T : struct, Enum
    {
        IDictionary<string, object> metadata = new ExpandoObject();
        var type = typeof(T);
        var attrs =
            (!Enum.IsDefined(value) ? (MemberInfo)type : type.GetField(Enum.GetName(value),
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            .GetCustomAttributes<MetadataAttribute>().FirstOrDefault((MetadataAttribute)null);
        string key = "";
        foreach (var v in attrs.Value)
        {
            if (Regex.IsMatch(v, @"^[A-Za-z_][A-Za-z0-9_]*$"))
            {
                if (!metadata.TryGetValue(key = v, out object o) || o is not StringBuilder)
                    metadata[key] = new StringBuilder();
            }
            else
            {
                if (metadata.TryGetValue(key, out object o) && o is StringBuilder builder)
                    builder.Append(v);
            }
        }
        foreach (var v in metadata.Keys)
        {
            key = metadata[v].ToString();
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
            metadata[v] = builder.ToString();
        }
        return metadata;
    }
}

