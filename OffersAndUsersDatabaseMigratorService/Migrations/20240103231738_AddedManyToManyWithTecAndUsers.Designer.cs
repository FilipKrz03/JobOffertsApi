﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OffersAndUsersDatabaseMigratorService.DbContexts;

#nullable disable

namespace OffersAndUsersDatabaseMigratorService.Migrations
{
    [DbContext(typeof(OffersApiDbContext))]
    [Migration("20240103231738_AddedManyToManyWithTecAndUsers")]
    partial class AddedManyToManyWithTecAndUsers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("JobOfferTechnology", b =>
                {
                    b.Property<Guid>("JobOffersId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TechnologiesId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("JobOffersId", "TechnologiesId");

                    b.HasIndex("TechnologiesId");

                    b.ToTable("JobOfferTechnology");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.JobOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EarningsFrom")
                        .HasColumnType("int");

                    b.Property<int?>("EarningsTo")
                        .HasColumnType("int");

                    b.Property<string>("Localization")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfferCompany")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfferLink")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfferTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Seniority")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("WorkMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("JobOffers");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.JobOfferUser", b =>
                {
                    b.Property<Guid>("JobOfferId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("JobOfferId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("JobOfferUsers");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.Technology", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("TechnologyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Technologies");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.TechnologyUser", b =>
                {
                    b.Property<Guid>("TechnologyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TechnologyId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TechnologyUsers");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("JobOfferTechnology", b =>
                {
                    b.HasOne("OffersAndUsersDatabaseMigratorService.Entities.JobOffer", null)
                        .WithMany()
                        .HasForeignKey("JobOffersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OffersAndUsersDatabaseMigratorService.Entities.Technology", null)
                        .WithMany()
                        .HasForeignKey("TechnologiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.JobOfferUser", b =>
                {
                    b.HasOne("OffersAndUsersDatabaseMigratorService.Entities.JobOffer", "JobOffer")
                        .WithMany("JobOfferUsers")
                        .HasForeignKey("JobOfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OffersAndUsersDatabaseMigratorService.Entities.User", "User")
                        .WithMany("JobOfferUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("JobOffer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.TechnologyUser", b =>
                {
                    b.HasOne("OffersAndUsersDatabaseMigratorService.Entities.Technology", "Technology")
                        .WithMany("TechnologyUsers")
                        .HasForeignKey("TechnologyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OffersAndUsersDatabaseMigratorService.Entities.User", "User")
                        .WithMany("TechnologyUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Technology");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.JobOffer", b =>
                {
                    b.Navigation("JobOfferUsers");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.Technology", b =>
                {
                    b.Navigation("TechnologyUsers");
                });

            modelBuilder.Entity("OffersAndUsersDatabaseMigratorService.Entities.User", b =>
                {
                    b.Navigation("JobOfferUsers");

                    b.Navigation("TechnologyUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
