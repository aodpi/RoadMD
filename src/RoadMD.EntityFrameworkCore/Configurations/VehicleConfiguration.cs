using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class VehicleConfiguration : BaseEntityConfiguration<Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasIndex(x => x.Number)
                .IsUnique();

            base.Configure(builder);
        }
    }
}