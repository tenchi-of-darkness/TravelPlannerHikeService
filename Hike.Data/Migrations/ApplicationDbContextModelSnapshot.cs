﻿// <auto-generated />
using System;
using System.Diagnostics.CodeAnalysis;
using Hike.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace Hike.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [ExcludeFromCodeCoverage]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Hike.Data.DBO.TrailDBO", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<long>("DistanceInMeters")
                        .HasColumnType("bigint");

                    b.Property<LineString>("LineString")
                        .IsRequired()
                        .HasColumnType("linestring");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("OwnerUserId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Trails");
                });

            modelBuilder.Entity("Hike.Data.DBO.UserDBO", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TrailDBOUserDBO", b =>
                {
                    b.Property<Guid>("FavoriteTrailsId")
                        .HasColumnType("char(36)");

                    b.Property<string>("UserDBOId")
                        .HasColumnType("varchar(255)");

                    b.HasKey("FavoriteTrailsId", "UserDBOId");

                    b.HasIndex("UserDBOId");

                    b.ToTable("TrailDBOUserDBO");
                });

            modelBuilder.Entity("TrailDBOUserDBO", b =>
                {
                    b.HasOne("Hike.Data.DBO.TrailDBO", null)
                        .WithMany()
                        .HasForeignKey("FavoriteTrailsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Hike.Data.DBO.UserDBO", null)
                        .WithMany()
                        .HasForeignKey("UserDBOId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
