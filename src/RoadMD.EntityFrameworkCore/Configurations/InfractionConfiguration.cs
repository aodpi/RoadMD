﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoadMD.Domain.Entities;

namespace RoadMD.EntityFrameworkCore.Configurations
{
    public class InfractionConfiguration : BaseEntityConfiguration<Infraction>
    {
        public override void Configure(EntityTypeBuilder<Infraction> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(1024);

            builder.HasOne(x => x.Location)
                .WithOne(x => x.Infraction)
                .HasForeignKey<Location>(x => x.InfractionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Photos)
                .WithOne(x => x.Infraction)
                .HasForeignKey(x => x.InfractionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Reports)
                .WithOne(x => x.Infraction)
                .HasForeignKey(x => x.InfractionId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            base.Configure(builder);
        }
    }
}