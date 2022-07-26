using FluentValidation.AspNetCore;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using RoadMD.Application.Dto.Infraction;
using RoadMD.Application.Dto.Infraction.List;
using RoadMD.Application.Dto.InfractionCategory;
using RoadMD.Application.Dto.ReportCategory;
using RoadMD.Application.Dto.Vehicle;
using RoadMD.Application.Services.InfractionCategories;
using RoadMD.Application.Services.Infractions;
using RoadMD.Application.Services.ReportCategories;
using RoadMD.Application.Services.Vehicles;
using RoadMD.Application.Validation.InfractionCategory;
using RoadMD.Application.Validation.ReportCategory;
using RoadMD.Application.Validation.Vehicle;
using RoadMD.Domain.Entities;
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
            services.AddControllers()
                .AddFluentValidation(cfg =>
                {
                    cfg.AutomaticValidationEnabled = true;
                    cfg.RegisterValidatorsFromAssemblyContaining<CreateVehicleValidator>();
                    cfg.RegisterValidatorsFromAssemblyContaining<CreateInfractionCategoryValidator>();
                    cfg.RegisterValidatorsFromAssemblyContaining<UpdateInfractionCategoryValidator>();

                    cfg.RegisterValidatorsFromAssemblyContaining<CreateReportCategoryValidator>();
                    cfg.RegisterValidatorsFromAssemblyContaining<UpdateReportCategoryValidator>();
                    cfg.DisableDataAnnotationsValidation = true;
                });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(f =>
            {
                f.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{nameof(RoadMD)}.xml"), true);
                f.CustomSchemaIds((type) =>
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

            ConfigureMappings(services);

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

        private static void ConfigureMappings(IServiceCollection services)
        {
            var config = new TypeAdapterConfig();

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            config.NewConfig<Vehicle, VehicleDto>();
            config.NewConfig<InfractionCategory, InfractionCategoryDto>();
            config.NewConfig<ReportCategory, ReportCategoryDto>();

            config.NewConfig<Infraction, InfractionDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CategoryId, src => src.CategoryId)
                .Map(dest => dest.Location, src => new InfractionLocationDto
                {
                    Longitude = src.Location.Longitude,
                    Latitude = src.Location.Latitude
                })
                .Map(dest => dest.Vehicle, src => new InfractionVehicleDto
                {
                    Number = src.Vehicle.Number
                })
                .Map(dest => dest.Photos, src => src.Photos.Select(photo => new InfractionPhotoDto
                {
                    Name = photo.Name,
                    Url = photo.Url
                }))
                .IgnoreNonMapped(true);

            config.NewConfig<Infraction, InfractionListDto>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.Location, src => new InfractionListLocationDto
                {
                    Longitude = src.Location.Longitude,
                    Latitude = src.Location.Latitude
                })
                .Map(dest => dest.Vehicle, src => new InfractionListVehicleDto
                {
                    Number = src.Vehicle.Number,
                })
                .IgnoreNonMapped(true);
        }
    }
}