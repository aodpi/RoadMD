using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Vehicle>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd().HasValueGenerator<GuidValueGenerator>();
        }
    }
}
