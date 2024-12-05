﻿// <auto-generated />
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("DAL.GameConfigurationDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardSizeHeight")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BoardSizeWidth")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChipsCountSerialized")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MovableBoardHeight")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovableBoardWidth")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MovePieceAfterNMoves")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WinCondition")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameConfigurations");
                });

            modelBuilder.Entity("DAL.GameStateDB", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BoardJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ChipsLeftJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GameConfigJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GridX")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GridY")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Player1Options")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Player2Options")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PlayersMovesJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Win")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("DAL.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
