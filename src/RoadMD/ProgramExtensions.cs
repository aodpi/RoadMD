using Azure.Storage.Blobs;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using RoadMD.Application;
using RoadMD.Application.Services.InfractionCategories;
using RoadMD.Application.Services.InfractionReports;
using RoadMD.Application.Services.Infractions;
using RoadMD.Application.Services.ReportCategories;
using RoadMD.Application.Services.Vehicles;
using RoadMD.EntityFrameworkCore;
using RoadMD.Module.AzurePhotoStorage;
using RoadMD.Module.PhotoStorage.Abstractions;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace RoadMD
{
    /// <summary>
    /// The "Startup.cs" we are missing
    /// </summary>
    public static class ProgramExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfigurationRoot configuration)
        {

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowAnyOrigin();
                });
            });

            services.AddRoadMdValidators();

            services.AddControllers();

            services.AddFluentValidationAutoValidation(validationConfiguration =>
                {
                    validationConfiguration.DisableDataAnnotationsValidation = true;
                })
                .AddFluentValidationClientsideAdapters();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(f =>
            {
                f.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{nameof(RoadMD)}.xml"), true);
                f.CustomSchemaIds(type =>
                {
                    var returnedValue = type.ShortDisplayName();

                    if (returnedValue.Contains("dto", StringComparison.InvariantCultureIgnoreCase))
                        returnedValue = returnedValue.Replace("dto", string.Empty,
                            StringComparison.InvariantCultureIgnoreCase);

                    return returnedValue;
                });
            });

            services.AddDbContext<ApplicationDbContext>(op =>
            {
                op.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddRoadMdMappings();
            services.AddRoadMdSieveProcessor();

            // Service factory for blob client
            var blobConnection  = configuration.GetConnectionString("BlobStorage");
            services.AddScoped(_ => new BlobServiceClient(blobConnection));
            services.AddScoped<IPhotoStorageService, AzurePhotoStorageService>();

            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IInfractionCategoriesService, InfractionCategoriesService>();
            services.AddScoped<IReportCategoryService, ReportCategoryService>();
            services.AddScoped<IInfractionService, InfractionService>();
            services.AddScoped<IInfractionReportService, InfractionReportService>();
        }

        public static void Configure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseCors();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.ConfigObject = new ConfigObject
                    {
                        DocExpansion = DocExpansion.None,
                        DisplayRequestDuration = true
                    };
                });
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.MapControllers();
        }
    }
}