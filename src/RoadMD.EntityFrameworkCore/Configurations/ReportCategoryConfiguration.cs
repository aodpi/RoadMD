using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class ReportCategoryConfiguration : BaseEntityConfiguration<ReportCategory>
    {
        public override void Configure(EntityTypeBuilder<ReportCategory> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(150);

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasMany(x => x.InfractionReports)
                .WithOne(x => x.ReportCategory)
                .HasForeignKey(x => x.ReportCategoryId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
}