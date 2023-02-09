using Microsoft.EntityFrameworkCore;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; } = default!;
        public DbSet<InfractionCategory> InfractionCategories { get; set; } = default!;
        public DbSet<ReportCategory> ReportCategories { get; set; } = default!;
        public DbSet<Infraction> Infractions { get; set; } = default!;
        public DbSet<Location> Locations { get; set; } = default!;
        public DbSet<Photo> Photos { get; set; } = default!;
        public DbSet<InfractionReport> InfractionReports { get; set; } = default!;
        public DbSet<Feedback> Feedbacks { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}