using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RoadMD.Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddValidatorsFromAssembly(executingAssembly);

            return services;
        }
    }
}