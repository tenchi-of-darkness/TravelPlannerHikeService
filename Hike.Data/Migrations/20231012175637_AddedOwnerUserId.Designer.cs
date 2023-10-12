﻿// <auto-generated />
using System;
using Hike.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;

#nullable disable

namespace Hike.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231012175637_AddedOwnerUserId")]
    partial class AddedOwnerUserId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Hike.Data.Entities.TrailEntity", b =>
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

                    b.Property<Guid>("OwnerUserId")
                        .HasColumnType("char(36)");

                    b.Property<float>("Rating")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Trails");
                });
#pragma warning restore 612, 618
        }
    }
}