﻿// <auto-generated />
using System;
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Migrations.AppDb
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.Car", b =>
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

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

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

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.CarBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarBrands");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "BMW"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Mercedes"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Audi"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Toyota"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Subaru"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Mitsubishi"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Ford"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Daweoo"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Lada"
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.CarColor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarColors");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Черный"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Белый"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Серый"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Красный"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Бордовый"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Зеленый"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Синий"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Фиолетовый"
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.CarType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CarTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Седан"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Минивэн"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Фургон"
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.ChatHub", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ConnectionId")
                        .HasColumnType("text");

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

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.ClientPackage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CarTypeId")
                        .HasColumnType("integer");

                    b.Property<int?>("ClientId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsSingle")
                        .HasColumnType("boolean");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int?>("OrderId")
                        .HasColumnType("integer");

                    b.Property<int?>("PackageId")
                        .HasColumnType("integer");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<int?>("RouteDateId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarTypeId");

                    b.HasIndex("ClientId");

                    b.HasIndex("LocationId");

                    b.HasIndex("OrderId");

                    b.HasIndex("PackageId");

                    b.HasIndex("RouteDateId");

                    b.ToTable("ClientPackages");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Driver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CarId")
                        .HasColumnType("integer");

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

                    b.Property<int?>("DriverId")
                        .HasColumnType("integer");

                    b.Property<int?>("KitId")
                        .HasColumnType("integer");

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

                    b.Property<bool>("IsUnlimited")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Kits");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsUnlimited = false,
                            Name = "Light",
                            Quantity = 5
                        },
                        new
                        {
                            Id = 2,
                            IsUnlimited = false,
                            Name = "Standard ",
                            Quantity = 10
                        },
                        new
                        {
                            Id = 3,
                            IsUnlimited = false,
                            Name = "Premium",
                            Quantity = 15
                        },
                        new
                        {
                            Id = 4,
                            IsUnlimited = true,
                            Name = "Unlimited",
                            Quantity = 999999
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Locations.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Locations.LocationDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("LocationDateTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int?>("RouteTripId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.HasIndex("RouteTripId");

                    b.ToTable("LocationDate");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("CancellationDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("CompletionDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<decimal>("OrderCost")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("RouteTripId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("StatusId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteTripId");

                    b.HasIndex("StatusId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Height")
                        .HasColumnType("double precision");

                    b.Property<double>("Length")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.Property<double>("Width")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.RejectOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClientPackageId")
                        .HasColumnType("integer");

                    b.Property<int?>("RouteTripId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientPackageId");

                    b.HasIndex("RouteTripId");

                    b.ToTable("RefusalOrders");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.RouteTrip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("DriverId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<int?>("RouteDateId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("RouteDateId");

                    b.ToTable("RouteTrips");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Cities");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Алматы"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Шымкент"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Астана"
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("FinishCityId")
                        .HasColumnType("integer");

                    b.Property<int>("StartCityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FinishCityId");

                    b.HasIndex("StartCityId");

                    b.ToTable("Routes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FinishCityId = 2,
                            StartCityId = 1
                        },
                        new
                        {
                            Id = 2,
                            FinishCityId = 3,
                            StartCityId = 1
                        },
                        new
                        {
                            Id = 3,
                            FinishCityId = 1,
                            StartCityId = 2
                        },
                        new
                        {
                            Id = 4,
                            FinishCityId = 1,
                            StartCityId = 3
                        },
                        new
                        {
                            Id = 5,
                            FinishCityId = 3,
                            StartCityId = 2
                        },
                        new
                        {
                            Id = 6,
                            FinishCityId = 2,
                            StartCityId = 3
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.RouteDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("DeliveryDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("RouteId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("RouteDate");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.RoutePrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.ToTable("RoutePrice");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Price = 1000m,
                            RouteId = 1
                        },
                        new
                        {
                            Id = 2,
                            Price = 2000m,
                            RouteId = 2
                        },
                        new
                        {
                            Id = 3,
                            Price = 1000m,
                            RouteId = 3
                        },
                        new
                        {
                            Id = 4,
                            Price = 2000m,
                            RouteId = 4
                        },
                        new
                        {
                            Id = 5,
                            Price = 2000m,
                            RouteId = 5
                        },
                        new
                        {
                            Id = 6,
                            Price = 2000m,
                            RouteId = 6
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("State")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Statuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            State = "New"
                        },
                        new
                        {
                            Id = 2,
                            State = "InProgress"
                        },
                        new
                        {
                            Id = 3,
                            State = "Done"
                        },
                        new
                        {
                            Id = 4,
                            State = "Delayed"
                        },
                        new
                        {
                            Id = 5,
                            State = "Canceled"
                        });
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.WaitingList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClientPackageId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ClientPackageId");

                    b.ToTable("WaitingList");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Cars.Car", b =>
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

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.ClientPackage", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Cars.CarType", "CarType")
                        .WithMany()
                        .HasForeignKey("CarTypeId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Order", null)
                        .WithMany("ClientPackages")
                        .HasForeignKey("OrderId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.RouteDate", "RouteDate")
                        .WithMany()
                        .HasForeignKey("RouteDateId");

                    b.Navigation("CarType");

                    b.Navigation("Client");

                    b.Navigation("Location");

                    b.Navigation("Package");

                    b.Navigation("RouteDate");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Driver", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Cars.Car", "Car")
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

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Locations.LocationDate", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Locations.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.RouteTrip", "RouteTrip")
                        .WithMany()
                        .HasForeignKey("RouteTripId");

                    b.Navigation("Location");

                    b.Navigation("RouteTrip");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Order", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.RouteTrip", "RouteTrip")
                        .WithMany()
                        .HasForeignKey("RouteTripId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Status", "Status")
                        .WithMany()
                        .HasForeignKey("StatusId");

                    b.Navigation("RouteTrip");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.RejectOrder", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.ClientPackage", "ClientPackage")
                        .WithMany()
                        .HasForeignKey("ClientPackageId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.RouteTrip", "RouteTrip")
                        .WithMany()
                        .HasForeignKey("RouteTripId");

                    b.Navigation("ClientPackage");

                    b.Navigation("RouteTrip");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.RouteTrip", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId");

                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.RouteDate", "RouteDate")
                        .WithMany()
                        .HasForeignKey("RouteDateId");

                    b.Navigation("Driver");

                    b.Navigation("RouteDate");
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

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Routes.RouteDate", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.Routes.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.Navigation("Route");
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

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.WaitingList", b =>
                {
                    b.HasOne("ApplicationCore.Entities.AppEntities.ClientPackage", "ClientPackage")
                        .WithMany()
                        .HasForeignKey("ClientPackageId");

                    b.Navigation("ClientPackage");
                });

            modelBuilder.Entity("ApplicationCore.Entities.AppEntities.Order", b =>
                {
                    b.Navigation("ClientPackages");
                });
#pragma warning restore 612, 618
        }
    }
}
