﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TypeRacerAPI.Data;

#nullable disable

namespace TypeRacerAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GameLevelId")
                        .HasColumnType("int");

                    b.Property<int>("GameTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsOpen")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOver")
                        .HasColumnType("bit");

                    b.Property<long>("StartTime")
                        .HasColumnType("bigint");

                    b.Property<string>("Words")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GameLevelId");

                    b.HasIndex("GameTypeId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameLevelBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GameLevelName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameLevel");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GameLevelName = "Beginner"
                        },
                        new
                        {
                            Id = 2,
                            GameLevelName = "Normal"
                        },
                        new
                        {
                            Id = 3,
                            GameLevelName = "Advanced"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameTypeBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("GameTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GameType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GameTypeName = "TimeAttack"
                        },
                        new
                        {
                            Id = 2,
                            GameTypeName = "FluentType"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrentWordIndex")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<bool>("IsPartyLeader")
                        .HasColumnType("bit");

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SocketID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WPM")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CooldownTime")
                        .HasColumnType("int");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsOneTimeUse")
                        .HasColumnType("bit");

                    b.Property<string>("PlayerPowerKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlayerPowerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PlayerPower");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CooldownTime = 0,
                            ImagePath = "/images/freeze.png",
                            IsOneTimeUse = false,
                            PlayerPowerKey = "F",
                            PlayerPowerName = "Freeze"
                        },
                        new
                        {
                            Id = 2,
                            CooldownTime = 0,
                            ImagePath = "/images/rewind.png",
                            IsOneTimeUse = false,
                            PlayerPowerKey = "R",
                            PlayerPowerName = "Rewind"
                        },
                        new
                        {
                            Id = 3,
                            CooldownTime = 0,
                            ImagePath = "/images/invisible.png",
                            IsOneTimeUse = false,
                            PlayerPowerKey = "I",
                            PlayerPowerName = "Invisible"
                        });
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerPowerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("PlayerPowerId");

                    b.ToTable("PlayerPowerUses");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameBase", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.GameLevelBase", "GameLevel")
                        .WithMany("Games")
                        .HasForeignKey("GameLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TypeRacerAPI.BaseClasses.GameTypeBase", "GameType")
                        .WithMany("Games")
                        .HasForeignKey("GameTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GameLevel");

                    b.Navigation("GameType");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerBase", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.GameBase", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerUse", b =>
                {
                    b.HasOne("TypeRacerAPI.BaseClasses.PlayerBase", "Player")
                        .WithMany("PlayerPowerUses")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TypeRacerAPI.BaseClasses.PlayerPowerBase", "PlayerPower")
                        .WithMany("PlayerPowerUses")
                        .HasForeignKey("PlayerPowerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("PlayerPower");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameBase", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameLevelBase", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.GameTypeBase", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerBase", b =>
                {
                    b.Navigation("PlayerPowerUses");
                });

            modelBuilder.Entity("TypeRacerAPI.BaseClasses.PlayerPowerBase", b =>
                {
                    b.Navigation("PlayerPowerUses");
                });
#pragma warning restore 612, 618
        }
    }
}
