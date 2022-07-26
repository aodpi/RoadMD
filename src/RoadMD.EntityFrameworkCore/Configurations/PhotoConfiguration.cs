using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class PhotoConfiguration : BaseEntityConfiguration<Photo>
    {
        public override void Configure(EntityTypeBuilder<Photo> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.BlobName)
                .IsRequired();

            builder.Property(x => x.Url)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasIndex(x => x.BlobName)
                .IsUnique();

            builder.HasIndex(x => x.Url)
                .IsUnique();

            builder.HasOne(x => x.Infraction)
                .WithMany(x => x.Photos)
                .HasForeignKey(x => x.InfractionId);

            base.Configure(builder);
        }
    }
}