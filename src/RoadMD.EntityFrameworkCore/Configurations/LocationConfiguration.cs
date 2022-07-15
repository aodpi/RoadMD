using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class LocationConfiguration : BaseEntityConfiguration<Location>
    {
        public override void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.Property(x => x.Latitude)
                .IsRequired();

            builder.Property(x => x.Longitude)
                .IsRequired();

            builder.HasOne(x => x.Infraction)
                .WithOne(x => x.Location)
                .HasForeignKey<Infraction>(x=>x.LocationId)
                .IsRequired();

            base.Configure(builder);
        }
    }
}