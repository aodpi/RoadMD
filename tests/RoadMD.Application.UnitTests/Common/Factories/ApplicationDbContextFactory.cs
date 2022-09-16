using Microsoft.EntityFrameworkCore;
using RoadMD.EntityFrameworkCore;

namespace RoadMD.Application.UnitTests.Common.Factories
{
    public static class ApplicationDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), builder => builder.EnableNullChecks(false))
                .Options;

            var context = new ApplicationDbContext(options);

            return context;
        }
    }
}