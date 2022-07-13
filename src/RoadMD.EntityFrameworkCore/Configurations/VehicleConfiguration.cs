using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class VehicleConfiguration : BaseEntityConfiguration<Vehicle>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.Property(x => x.LetterCode)
                .IsRequired()
                .HasMaxLength(5);

            builder.Property(x => x.NumberCode)
                .IsRequired()
                .HasMaxLength(5);

            base.Configure(builder);
        }
    }
}