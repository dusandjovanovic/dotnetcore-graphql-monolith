﻿// <auto-generated />
using System;
using GraphQL.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GraphQL.API.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GraphQL.Core.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Location", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Place", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LocationId");

                    b.ToTable("Places");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("LocationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("Modified")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("PlaceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("LocationId");

                    b.HasIndex("PlaceId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Account", b =>
                {
                    b.HasOne("GraphQL.Core.Models.Account", null)
                        .WithMany("Friends")
                        .HasForeignKey("AccountId");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Place", b =>
                {
                    b.HasOne("GraphQL.Core.Models.Account", null)
                        .WithMany("AppearsIn")
                        .HasForeignKey("AccountId");

                    b.HasOne("GraphQL.Core.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Tag", b =>
                {
                    b.HasOne("GraphQL.Core.Models.Account", null)
                        .WithMany("SharedTags")
                        .HasForeignKey("AccountId");

                    b.HasOne("GraphQL.Core.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("GraphQL.Core.Models.Place", null)
                        .WithMany("Tags")
                        .HasForeignKey("PlaceId");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Account", b =>
                {
                    b.Navigation("AppearsIn");

                    b.Navigation("Friends");

                    b.Navigation("SharedTags");
                });

            modelBuilder.Entity("GraphQL.Core.Models.Place", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
