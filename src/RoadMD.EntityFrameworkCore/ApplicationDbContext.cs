using Microsoft.EntityFrameworkCore;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<InfractionCategory> InfractionCategories { get; set; }
        public DbSet<ReportCategory> ReportCategories { get; set; }
        public DbSet<Infraction> Infractions { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}