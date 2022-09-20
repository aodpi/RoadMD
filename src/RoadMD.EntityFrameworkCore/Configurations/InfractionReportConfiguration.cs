using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class InfractionReportConfiguration : BaseEntityConfiguration<InfractionReport>
    {
        /// <inheritdoc />
        public override void Configure(EntityTypeBuilder<InfractionReport> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.HasOne(x => x.Infraction)
                .WithMany(x => x.Reports)
                .HasForeignKey(x => x.InfractionId);

            builder.HasOne(x => x.ReportCategory)
                .WithMany(x => x.InfractionReports)
                .HasForeignKey(x => x.ReportCategoryId);
        }
    }
}