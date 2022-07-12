namespace RoadMD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure app-wide services
            builder.Services.ConfigureServices();
            
            var app = builder.Build();

            // Configure web application
            app.Configure();

            app.Run();
        }
    }
}