using Microsoft.EntityFrameworkCore;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.UnitTests.Common.Factories
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer("Server=localhost;Database=RoadMD;Trusted_Connection=True;TrustServerCertificate=True;")
                /*.UseSqlite($"DataSource=RoadMD_{Guid.NewGuid()}.db", builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                })*/
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreatedAsync();
            return context;
        }
    }
}