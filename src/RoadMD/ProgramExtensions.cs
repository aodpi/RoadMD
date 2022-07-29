using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application;
using RoadMD.Application.Services.InfractionCategories;
using RoadMD.Application.Services.Infractions;
using RoadMD.Application.Services.ReportCategories;
using RoadMD.Application.Services.Vehicles;
using RoadMD.EntityFrameworkCore;
using RoadMD.Module.EmailSender;
using RoadMD.Modules.Abstractions;

namespace RoadMD
{
    /// <summary>
    /// The "Startup.cs" we are missing
    /// </summary>
    public static class ProgramExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddRoadMdValidators();

            services.AddControllers()
                .AddFluentValidation(cfg =>
                {
                    cfg.AutomaticValidationEnabled = true;
                    cfg.DisableDataAnnotationsValidation = true;
                });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(f =>
            {
                f.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{nameof(RoadMD)}.xml"), true);
                f.CustomSchemaIds(type =>
                {
                    var returnedValue = type.Name;

                    if (returnedValue.EndsWith("dto", StringComparison.InvariantCultureIgnoreCase))
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
          
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IInfractionCategoriesService, InfractionCategoriesService>();
            services.AddScoped<IReportCategoryService, ReportCategoryService>();
            services.AddScoped<IInfractionService, InfractionService>();
            services.AddScoped<IEmailSender, EmailSenderMailKit>();
        }

        public static void Configure(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.MapControllers();
        }
    }
}