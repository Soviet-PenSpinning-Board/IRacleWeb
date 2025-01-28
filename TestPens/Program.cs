using System.Text.Json;
using System.Text.Json.Serialization;

using TestPens.Extensions;
using TestPens.Service;
using TestPens.Service.Abstractions;

using Westwind.AspNetCore.Markdown;

namespace TestPens
{
    public class Program
    {
        public static JsonSerializerOptions JsonOptions = new();

        public static void Main(string[] args)
        {
            JsonOptions.Converters.Add(new JsonStringEnumConverter());
            JsonOptions.Converters.Add(new ChangesConverter());

            JsonOptions.AllowTrailingCommas = true;
            JsonOptions.PropertyNameCaseInsensitive = true;

            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSingleton<IPersonContainerService, JsonPersonContainerService>();
            builder.Services.AddSingleton<IBattleControllerService, JsonBattleControllerService>();

            builder.Services.AddSingleton<ITokenManager, JsonTokenManager>();

            builder.Services.AddMarkdown();

            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new ChangesConverter());
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseMarkdown();
            app.UseStaticFiles();

            app.UseRouting();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Main}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
