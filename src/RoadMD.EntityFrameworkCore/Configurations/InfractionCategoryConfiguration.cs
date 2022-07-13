using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class InfractionCategoryConfiguration : BaseEntityConfiguration<InfractionCategory>
    {
        public override void Configure(EntityTypeBuilder<InfractionCategory> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.HasIndex(x => x.Name)
                .IsUnique(true);

            base.Configure(builder);
        }
    }
}