using System.Reflection;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using RoadMD.Application.Common.Sieve;
using Sieve.Services;

namespace RoadMD.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddRoadMdValidators(this IServiceCollection services)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(executingAssembly);

            return services;
        }

        public static IServiceCollection AddRoadMdMappings(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();

            config.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }

        public static IServiceCollection AddRoadMdSieveProcessor(this IServiceCollection services)
        {
            services.AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>();
            services.AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>();
            services.AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            return services;
        }
    }
}