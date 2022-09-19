using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class FeedbackConfiguration : BaseEntityConfiguration<Feedback>
    {
        public override void Configure(EntityTypeBuilder<Feedback> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(1024);

            builder.Property(x => x.UserName)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.UserEmail)
                .IsRequired()
                .HasMaxLength(128);
        }
    }
}