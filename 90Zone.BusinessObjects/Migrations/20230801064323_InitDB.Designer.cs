﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using _90Zone.BusinessObjects.Models;

#nullable disable

namespace _90Zone.BusinessObjects.Migrations
{
    [DbContext(typeof(_90ZoneDbContext))]
    [Migration("20230801064323_InitDB")]
    partial class InitDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Club", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Chairmen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Coach")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stadium")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("ManagerId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Continent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Manager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Managers");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CurrentClubId")
                        .HasColumnType("int");

                    b.Property<int?>("Height")
                        .HasColumnType("int");

                    b.Property<string>("ImgPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsInjured")
                        .HasColumnType("bit");

                    b.Property<int?>("JerseyNumber")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NationalityId")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Value")
                        .HasColumnType("float");

                    b.Property<int?>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrentClubId");

                    b.HasIndex("NationalityId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Club", b =>
                {
                    b.HasOne("_90Zone.BusinessObjects.Models.League", "League")
                        .WithMany("Clubs")
                        .HasForeignKey("LeagueId");

                    b.HasOne("_90Zone.BusinessObjects.Models.Manager", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");

                    b.Navigation("League");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.League", b =>
                {
                    b.HasOne("_90Zone.BusinessObjects.Models.Country", "Country")
                        .WithMany("Leagues")
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Player", b =>
                {
                    b.HasOne("_90Zone.BusinessObjects.Models.Club", "CurrentClub")
                        .WithMany("Players")
                        .HasForeignKey("CurrentClubId");

                    b.HasOne("_90Zone.BusinessObjects.Models.Country", "Nationality")
                        .WithMany("Players")
                        .HasForeignKey("NationalityId");

                    b.Navigation("CurrentClub");

                    b.Navigation("Nationality");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Club", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.Country", b =>
                {
                    b.Navigation("Leagues");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("_90Zone.BusinessObjects.Models.League", b =>
                {
                    b.Navigation("Clubs");
                });
#pragma warning restore 612, 618
        }
    }
}
