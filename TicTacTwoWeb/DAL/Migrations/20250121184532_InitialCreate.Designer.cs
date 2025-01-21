﻿// <auto-generated />
using System;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250121184532_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DAL.GameConfiguration", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("BoardSizeHeight")
                        .HasColumnType("integer");

                    b.Property<int>("BoardSizeWidth")
                        .HasColumnType("integer");

                    b.Property<string>("ChipsCountJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<int>("MovableBoardHeight")
                        .HasColumnType("integer");

                    b.Property<int>("MovableBoardWidth")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OptionsAfterNMoves")
                        .HasColumnType("integer");

                    b.Property<int>("WinCondition")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("GameConfigurations");
                });

            modelBuilder.Entity("DAL.GameSession", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("GameConfigId")
                        .HasColumnType("text");

                    b.Property<string>("GameMode")
                        .HasColumnType("text");

                    b.Property<string>("GamePassword")
                        .HasColumnType("text");

                    b.Property<string>("GameStateId")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastSaveAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Player1Id")
                        .HasColumnType("text");

                    b.Property<string>("Player2Id")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameConfigId");

                    b.HasIndex("GameStateId");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.ToTable("GameSessions");
                });

            modelBuilder.Entity("DAL.GameState", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BoardJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ChipsLeftJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GridX")
                        .HasColumnType("integer");

                    b.Property<int>("GridY")
                        .HasColumnType("integer");

                    b.Property<bool>("Player1Options")
                        .HasColumnType("boolean");

                    b.Property<bool>("Player2Options")
                        .HasColumnType("boolean");

                    b.Property<int>("PlayerNumber")
                        .HasColumnType("integer");

                    b.Property<string>("PlayersMovesJson")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Win")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("GameStates");
                });

            modelBuilder.Entity("DAL.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL.GameConfiguration", b =>
                {
                    b.HasOne("DAL.User", "User")
                        .WithMany()
                        .HasForeignKey("CreatedBy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.GameSession", b =>
                {
                    b.HasOne("DAL.GameConfiguration", "GameConfiguration")
                        .WithMany()
                        .HasForeignKey("GameConfigId");

                    b.HasOne("DAL.GameState", "GameState")
                        .WithMany()
                        .HasForeignKey("GameStateId");

                    b.HasOne("DAL.User", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id");

                    b.HasOne("DAL.User", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id");

                    b.Navigation("GameConfiguration");

                    b.Navigation("GameState");

                    b.Navigation("Player1");

                    b.Navigation("Player2");
                });
#pragma warning restore 612, 618
        }
    }
}
