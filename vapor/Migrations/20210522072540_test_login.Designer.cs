﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vapor.Data;

namespace vapor.Migrations
{
    [DbContext(typeof(vaporContext))]
    [Migration("20210522072540_test_login")]
    partial class test_login
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.Property<string>("gamesid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("generesid")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("gamesid", "generesid");

                    b.HasIndex("generesid");

                    b.ToTable("GameGenre");
                });

            modelBuilder.Entity("vapor.Models.Customer", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("firstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("lastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("vapor.Models.Developer", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Developer");
                });

            modelBuilder.Entity("vapor.Models.Game", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("developerid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<DateTime>("releaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("developerid");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("vapor.Models.GameImage", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("gameid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("imageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("gameid");

                    b.ToTable("GameImage");
                });

            modelBuilder.Entity("vapor.Models.Genre", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Genre");
                });

            modelBuilder.Entity("vapor.Models.Order", b =>
                {
                    b.Property<string>("customerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("gameId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.HasKey("customerId", "gameId");

                    b.HasIndex("gameId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("vapor.Models.Review", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("cusotmerid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("gameid")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("lastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<float>("rating")
                        .HasColumnType("real");

                    b.Property<DateTime>("writtenAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("cusotmerid");

                    b.HasIndex("gameid");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("vapor.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GameGenre", b =>
                {
                    b.HasOne("vapor.Models.Game", null)
                        .WithMany()
                        .HasForeignKey("gamesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vapor.Models.Genre", null)
                        .WithMany()
                        .HasForeignKey("generesid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("vapor.Models.Game", b =>
                {
                    b.HasOne("vapor.Models.Developer", "developer")
                        .WithMany("games")
                        .HasForeignKey("developerid");

                    b.Navigation("developer");
                });

            modelBuilder.Entity("vapor.Models.GameImage", b =>
                {
                    b.HasOne("vapor.Models.Game", "game")
                        .WithMany("images")
                        .HasForeignKey("gameid");

                    b.Navigation("game");
                });

            modelBuilder.Entity("vapor.Models.Order", b =>
                {
                    b.HasOne("vapor.Models.Customer", "customer")
                        .WithMany("orders")
                        .HasForeignKey("customerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("vapor.Models.Game", "game")
                        .WithMany()
                        .HasForeignKey("gameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("customer");

                    b.Navigation("game");
                });

            modelBuilder.Entity("vapor.Models.Review", b =>
                {
                    b.HasOne("vapor.Models.Customer", "cusotmer")
                        .WithMany("reviews")
                        .HasForeignKey("cusotmerid");

                    b.HasOne("vapor.Models.Game", "game")
                        .WithMany("reviews")
                        .HasForeignKey("gameid");

                    b.Navigation("cusotmer");

                    b.Navigation("game");
                });

            modelBuilder.Entity("vapor.Models.Customer", b =>
                {
                    b.Navigation("orders");

                    b.Navigation("reviews");
                });

            modelBuilder.Entity("vapor.Models.Developer", b =>
                {
                    b.Navigation("games");
                });

            modelBuilder.Entity("vapor.Models.Game", b =>
                {
                    b.Navigation("images");

                    b.Navigation("reviews");
                });
#pragma warning restore 612, 618
        }
    }
}