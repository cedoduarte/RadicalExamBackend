using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using RadicalExam.Services;

namespace RadicalExam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddSingleton<IConfiguration>(options =>
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile("appsettings.Local.json", true)
                    .AddEnvironmentVariables();
                return configurationBuilder.Build();
            });
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });
            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(builder.Configuration["ApiVersion"], new OpenApiInfo()
                {
                    Version = builder.Configuration["ApiVersion"],
                    Title = builder.Configuration["ApiTitle"],
                    Description = builder.Configuration["ApiDescription"],
                    TermsOfService = new Uri(builder.Configuration["ApiTermsOfServiceUrl"]),
                    Contact = new OpenApiContact()
                    {
                        Name = builder.Configuration["ApiContactName"],
                        Url = new Uri(builder.Configuration["ApiContactUrl"])
                    },
                    License = new OpenApiLicense()
                    {
                        Name = builder.Configuration["ApiLicense"],
                        Url = new Uri(builder.Configuration["ApiLicenseUrl"])
                    }
                });
            });
            builder.Services.AddTransient<IBanxicoService, BanxicoService>();
            builder.Services.AddTransient<IWeatherTemperatureService, WeatherTemperatureService>();
            builder.Services.AddTransient<IExcelFileProcessorService, ExcelFileProcessorService>();
            builder.Services.AddMvc();
            var app = builder.Build();
            app.UseCors(options =>
                options.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod());
            app.UseStaticFiles();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(builder.Configuration["ApiSwaggerEndpoint"], builder.Configuration["ApiVersion"]);
                });
            }
            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}