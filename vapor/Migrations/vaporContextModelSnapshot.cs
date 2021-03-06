// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using vapor.Data;

namespace vapor.Migrations
{
    [DbContext(typeof(vaporContext))]
    partial class vaporContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("genresid")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("gamesid", "genresid");

                    b.HasIndex("genresid");

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

                    b.Property<string>("fileContentType")
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

                    b.Property<string>("developerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("price")
                        .HasColumnType("float");

                    b.Property<DateTime>("releaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("developerId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("vapor.Models.GameImage", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("fileBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fileContentType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("gameID")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("id");

                    b.HasIndex("gameID");

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

            modelBuilder.Entity("vapor.Models.MapCoordinates", b =>
                {
                    b.Property<string>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("latitude")
                        .HasColumnType("float");

                    b.Property<double>("longitude")
                        .HasColumnType("float");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("MapCoordinates");
                });

            modelBuilder.Entity("vapor.Models.Order", b =>
                {
                    b.Property<string>("customerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("gameId")
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

                    b.Property<string>("customerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("gameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("lastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<float>("rating")
                        .HasColumnType("real");

                    b.Property<DateTime>("writtenAt")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("customerId");

                    b.HasIndex("gameId");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("vapor.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("customerID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("developerID")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("customerID");

                    b.HasIndex("developerID");

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
                        .HasForeignKey("genresid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("vapor.Models.Game", b =>
                {
                    b.HasOne("vapor.Models.Developer", "developer")
                        .WithMany("games")
                        .HasForeignKey("developerId");

                    b.Navigation("developer");
                });

            modelBuilder.Entity("vapor.Models.GameImage", b =>
                {
                    b.HasOne("vapor.Models.Game", "game")
                        .WithMany("images")
                        .HasForeignKey("gameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

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
                        .HasForeignKey("customerId");

                    b.HasOne("vapor.Models.Game", "game")
                        .WithMany("reviews")
                        .HasForeignKey("gameId");

                    b.Navigation("cusotmer");

                    b.Navigation("game");
                });

            modelBuilder.Entity("vapor.Models.User", b =>
                {
                    b.HasOne("vapor.Models.Customer", "customer")
                        .WithMany()
                        .HasForeignKey("customerID");

                    b.HasOne("vapor.Models.Developer", "developer")
                        .WithMany()
                        .HasForeignKey("developerID");

                    b.Navigation("customer");

                    b.Navigation("developer");
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
