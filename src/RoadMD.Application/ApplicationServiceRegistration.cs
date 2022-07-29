using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

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
    }
}