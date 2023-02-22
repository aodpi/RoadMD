using Serilog;

namespace RoadMD
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure app-wide services
            builder.Services.ConfigureServices(builder.Configuration);

            builder.Host.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });
            
            var app = builder.Build();

            // Configure web application
            app.Configure();

            app.Run();
        }
    }
}