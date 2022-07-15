﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RoadMD.EntityFrameworkCore;

#nullable disable

namespace RoadMD.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220715070515_AddedInfractionCategoriesEntity")]
    partial class AddedInfractionCategoriesEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("RoadMD.Domain.Entities.InfractionCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("InfractionCategories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("5a4a7870-e95c-4443-883d-d2126a5efa04"),
                            Name = "Conducerea unui vehicul fără număr de înmatriculare"
                        },
                        new
                        {
                            Id = new Guid("4940a717-eca6-4a9e-9374-a34a5f46e279"),
                            Name = "Amplasarea ilegală pe vehicul a unui număr de înmatriculare fals"
                        },
                        new
                        {
                            Id = new Guid("fbe7c0c8-fcda-4314-860d-d3e1f2c29cd7"),
                            Name = "Depăşirea vitezei de circulaţie stabilită"
                        },
                        new
                        {
                            Id = new Guid("796b8081-bb01-45f2-a76e-0d7a03250914"),
                            Name = "Oprirea în locuri interzise"
                        },
                        new
                        {
                            Id = new Guid("d7d5ac06-54b1-49a3-9d1f-539e5a48ccd9"),
                            Name = "Staţionarea sau parcarea în locuri interzise"
                        });
                });

            modelBuilder.Entity("RoadMD.Domain.Entities.Vehicle", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LetterCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("NumberCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
