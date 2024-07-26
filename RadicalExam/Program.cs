using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using RadicalExam.Middlewares;
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

            builder.Services.AddTransient<IExcelFileProcessorService, ExcelFileProcessorService>();
            /*
            builder.Services.AddTransient<IAppDbContext, AppDbContext>();
            builder.Services.AddTransient<IArtExhibitionService, ArtExhibitionService>();
            builder.Services.AddTransient<IArtisticWorkService, ArtisticWorkService>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ITextService, TextService>();
            builder.Services.AddTransient<IThumbnailService, ThumbnailService>();
            builder.Services.AddTransient<IUserService, UserService>();
            */

            /*
            builder.Services.AddSingleton(new MapperConfiguration(configuration =>
            {
                configuration.AddProfile(new MappingProfile());
            }).CreateMapper());
            */

            /*
            builder.Services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });
            */

            /*
            string dbConnectionString = builder.Configuration.GetConnectionString("luisdiego");
            builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
            {
                options.UseMySql(dbConnectionString, ServerVersion.AutoDetect(dbConnectionString), sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(AppDbContext).GetTypeInfo().Assembly.GetName().Name);
                });
            });
            */

            builder.Services.AddMvc();
            var app = builder.Build();
            
            /*
            using (IServiceScope scope = app.Services.CreateScope())
            {
                try
                {
                    AppDbContext dbContext = (AppDbContext)scope.ServiceProvider.GetService<IAppDbContext>();
                    dbContext.Database.Migrate();
                    DbSeeder.DoSeeding(dbContext);
                }
                catch (Exception ex)
                {
                    ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error ocurred while migrating or initializing the database");
                }
            }
            */

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
            //app.UseAuthorizationToken(builder.Configuration["AuthorizationToken"]);
            app.MapControllers();
            app.Run();
        }
    }
}