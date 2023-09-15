﻿// <auto-generated />
using System;
using Infrastructure.Context.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230704103502_InitialAppDb1")]
    partial class InitialAppDb1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.CarBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarBrands");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.CarColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarColors");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.CarType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarTypes");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.ChatHub", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ChatHubs");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CarId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DriverLicenceScanPath")
                        .HasColumnType("text");

                    b.Property<string>("IdentificationNumber")
                        .HasColumnType("text");

                    b.Property<string>("IdentificationSeries")
                        .HasColumnType("text");

                    b.Property<DateTime>("IdentityCardCreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("IdentityCardPhotoPath")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.DriverKit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DriverId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int?>("KitId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("KitId");

                    b.ToTable("DriversKits");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Kit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUnlimited")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Kits");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Locations.LocationData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DeliveryId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LocationDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryId");

                    b.HasIndex("LocationId");

                    b.ToTable("LocationData");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Orders.Delivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("CancellationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DriverId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("RouteId")
                        .HasColumnType("integer");

                    b.Property<int?>("StateId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("LocationId");

                    b.HasIndex("RouteId");

                    b.HasIndex("StateId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Orders.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Height")
                        .HasPrecision(18, 2)
                        .HasColumnType("double precision");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<double>("Length")
                        .HasPrecision(18, 2)
                        .HasColumnType("double precision");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("Weight")
                        .HasPrecision(18, 2)
                        .HasColumnType("double precision");

                    b.Property<double>("Width")
                        .HasPrecision(18, 2)
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Orders.RejectedOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DeliveryId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryId");

                    b.HasIndex("OrderId");

                    b.ToTable("RejectedOrders");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("FinishCityId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("StartCityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FinishCityId");

                    b.HasIndex("StartCityId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.RoutePrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Price")
                        .HasColumnType("double precision");

                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("RoutePrice");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.UIMessages.MessageForUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("MessagesForUser");
                });

            modelBuilder.Entity("ApplicationCore.Models.Entities.Cars.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CarBrandId")
                        .HasColumnType("integer");

                    b.Property<int?>("CarColorId")
                        .HasColumnType("integer");

                    b.Property<string>("CarNumber")
                        .HasColumnType("text");

                    b.Property<int?>("CarTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("ProductionYear")
                        .HasColumnType("integer");

                    b.Property<string>("RegistrationCertificate")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CarBrandId");

                    b.HasIndex("CarColorId");

                    b.HasIndex("CarTypeId");

                    b.ToTable("Cars");
                });

            modelBuilder.Entity("ApplicationCore.Models.Entities.Locations.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ApplicationCore.Models.Entities.Orders.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AddressFrom")
                        .HasColumnType("text");

                    b.Property<string>("AddressTo")
                        .HasColumnType("text");

                    b.Property<int?>("CarTypeId")
                        .HasColumnType("integer");

                    b.Property<int?>("ClientId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DeliveryId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSingle")
                        .HasColumnType("boolean");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("PackageId")
                        .HasColumnType("integer");

                    b.Property<double>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("double precision");

                    b.Property<int?>("RouteId")
                        .HasColumnType("integer");

                    b.Property<string>("SecretCode")
                        .HasColumnType("text");

                    b.Property<int?>("StateId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarTypeId");

                    b.HasIndex("ClientId");

                    b.HasIndex("DeliveryId");

                    b.HasIndex("LocationId");

                    b.HasIndex("PackageId");

                    b.HasIndex("RouteId");

                    b.HasIndex("StateId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ApplicationCore.Models.Entities.Orders.State", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("StateName")
                        .HasColumnType("text");

                    b.Property<int>("StateValue")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Driver", b =>
                {
                    b.HasOne("ApplicationCore.Models.Entities.Cars.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarId");

                    b.Navigation("Car");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.DriverKit", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Kit", "Kit")
                        .WithMany()
                        .HasForeignKey("KitId");

                    b.Navigation("Driver");

                    b.Navigation("Kit");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Locations.LocationData", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Orders.Delivery", "Delivery")
                        .WithMany()
                        .HasForeignKey("DeliveryId");

                    b.HasOne("ApplicationCore.Models.Entities.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.Navigation("Delivery");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Orders.Delivery", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId");

                    b.HasOne("ApplicationCore.Models.Entities.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.HasOne("ApplicationCore.Models.Entities.Orders.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId");

                    b.Navigation("Driver");

                    b.Navigation("Location");

                    b.Navigation("Route");

                    b.Navigation("State");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Orders.RejectedOrder", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Orders.Delivery", "Delivery")
                        .WithMany()
                        .HasForeignKey("DeliveryId");

                    b.HasOne("ApplicationCore.Models.Entities.Orders.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId");

                    b.Navigation("Delivery");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.Route", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.City", "FinishCity")
                        .WithMany()
                        .HasForeignKey("FinishCityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.City", "StartCity")
                        .WithMany()
                        .HasForeignKey("StartCityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FinishCity");

                    b.Navigation("StartCity");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.RoutePrice", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");
                });

            modelBuilder.Entity("ApplicationCore.Models.Entities.Cars.Car", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Cars.CarBrand", "CarBrand")
                        .WithMany()
                        .HasForeignKey("CarBrandId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Cars.CarColor", "CarColor")
                        .WithMany()
                        .HasForeignKey("CarColorId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Cars.CarType", "CarType")
                        .WithMany()
                        .HasForeignKey("CarTypeId");

                    b.Navigation("CarBrand");

                    b.Navigation("CarColor");

                    b.Navigation("CarType");
                });

            modelBuilder.Entity("ApplicationCore.Models.Entities.Orders.Order", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Cars.CarType", "CarType")
                        .WithMany()
                        .HasForeignKey("CarTypeId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Orders.Delivery", "Delivery")
                        .WithMany("Orders")
                        .HasForeignKey("DeliveryId");

                    b.HasOne("ApplicationCore.Models.Entities.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Orders.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.HasOne("ApplicationCore.Models.Entities.Orders.State", "State")
                        .WithMany()
                        .HasForeignKey("StateId");

                    b.Navigation("CarType");

                    b.Navigation("Client");

                    b.Navigation("Delivery");

                    b.Navigation("Location");

                    b.Navigation("Package");

                    b.Navigation("Route");

                    b.Navigation("State");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Orders.Delivery", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
