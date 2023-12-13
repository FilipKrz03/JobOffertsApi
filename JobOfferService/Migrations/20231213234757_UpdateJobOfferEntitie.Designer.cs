﻿// <auto-generated />
using System;
using JobOffersService.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobOffersService.Migrations
{
    [DbContext(typeof(JobOffersContext))]
    [Migration("20231213234757_UpdateJobOfferEntitie")]
    partial class UpdateJobOfferEntitie
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("JobOffersService.Entities.JobOffer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

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

                    b.Property<string>("WorkMode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("JobOffers");
                });

            modelBuilder.Entity("JobOffersService.Entities.Technology", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TechnologyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Technologies");
                });

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

            modelBuilder.Entity("JobOfferTechnology", b =>
                {
                    b.HasOne("JobOffersService.Entities.JobOffer", null)
                        .WithMany()
                        .HasForeignKey("JobOffersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JobOffersService.Entities.Technology", null)
                        .WithMany()
                        .HasForeignKey("TechnologiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
