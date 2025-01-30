using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Options;

using TestPens.Extensions;
using TestPens.Models.Dto.Changes;
using TestPens.Models.Real.Changes;
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
            JsonOptions.Converters.Add(new ChangesDtoConverter());
            JsonOptions.Converters.Add(new ChangesModelConverter());

            JsonOptions.AllowTrailingCommas = true;
            JsonOptions.PropertyNameCaseInsensitive = true;

            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddEnvironmentVariables();

            builder.Services.AddSingleton<IPersonContainerService, JsonPersonContainerService>();
            builder.Services.AddSingleton<IBattleControllerService, JsonBattleControllerService>();

            builder.Services.AddSingleton<ITokenManager, JsonTokenManager>();

            builder.Services.AddMarkdown();

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(opt =>
                {
                    opt.UseOneOfForPolymorphism();
                    opt.SelectDiscriminatorNameUsing(subType =>
                    {
                        if (!typeof(ChangeBaseDto).IsAssignableFrom(subType) && !typeof(ChangeBaseModel).IsAssignableFrom(subType))
                        {
                            return null;
                        }

                        return "type";
                    });
                    opt.SelectDiscriminatorValueUsing(subType =>
                    {
                        if (!typeof(ChangeBaseDto).IsAssignableFrom(subType) && !typeof(ChangeBaseModel).IsAssignableFrom(subType))
                        {
                            return null;
                        }

                        object obj = Activator.CreateInstance(subType)!;
                        return subType.GetProperty("Type")!.GetValue(obj)!.ToString();
                    });

                    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                });


            }

            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new ChangesDtoConverter());
                    options.JsonSerializerOptions.Converters.Add(new ChangesModelConverter());
                    options.JsonSerializerOptions.AllowTrailingCommas = true;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                });

            var app = builder.Build();

            if (builder.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
