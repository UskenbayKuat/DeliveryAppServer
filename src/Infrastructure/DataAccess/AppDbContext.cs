using ApplicationCore.Entities.AppEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public class AppDbContext : DbContext
    {
        //users
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Client> Clients { get; set; }

        //cars
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<CarBrand> CarBrands { get; set; }
        public DbSet<CarColor> CarColors { get; set; }

        //routes
        public DbSet<City> Cities { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<RouteDate> RouteDate { get; set; }
        public DbSet<RoutePrice> RoutePrice { get; set; }

        public DbSet<RouteTrip> RouteTrips { get; set; }

        //locations
        public DbSet<Location> Locations { get; set; }

        public DbSet<LocationDate> LocationDate { get; set; }

        //orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<ClientPackage> ClientPackages { get; set; }
        public DbSet<WaitingList> WaitingList { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Status> Statuses { get; set; }

        //kits
        public DbSet<Kit> Kits { get; set; }
        public DbSet<DriverKit> DriversKits { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientPackage>().Property(c => c.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.OrderCost).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CarType>().HasData(
                new CarType { Id = 1, Name = "Седан" },
                new CarType { Id = 2, Name = "Минивэн" },
                new CarType { Id = 3, Name = "Фургон" }
            );

            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Алматы" },
                new City { Id = 2, Name = "Шымкент" },
                new City { Id = 3, Name = "Нур-Султан" }
            );

            modelBuilder.Entity<Kit>().HasData(
                new Kit { Id = 1, Name = "Light", Quantity = 5, IsUnlimited = false },
                new Kit { Id = 2, Name = "Standard ", Quantity = 10, IsUnlimited = false },
                new Kit { Id = 3, Name = "Premium", Quantity = 15, IsUnlimited = false },
                new Kit { Id = 4, Name = "Unlimited", Quantity = 999999, IsUnlimited = true }
            );


            modelBuilder.Entity<RoutePrice>().HasData(
                new RoutePrice { Id = 1, RouteId = 1, Price = 1000 },
                new RoutePrice { Id = 2, RouteId = 2, Price = 2000 },
                new RoutePrice { Id = 3, RouteId = 3, Price = 1000 },
                new RoutePrice { Id = 4, RouteId = 4, Price = 2000 },
                new RoutePrice { Id = 5, RouteId = 5, Price = 2000 },
                new RoutePrice { Id = 6, RouteId = 6, Price = 2000 }
            );
            modelBuilder.Entity<Route>().HasData(
                new Route { Id = 1, StartCityId = 1 , FinishCityId = 2  },
                new Route { Id = 2, StartCityId = 1 , FinishCityId = 3  },
                new Route { Id = 3, StartCityId = 2 , FinishCityId = 1  },
                new Route { Id = 4, StartCityId = 3 , FinishCityId = 1  },
                new Route { Id = 5, StartCityId = 2 , FinishCityId = 3  },
                new Route { Id = 6, StartCityId = 3 , FinishCityId = 2  }
            );
            modelBuilder.Entity<CarBrand>().HasData(
                new CarBrand { Id = 1, Name = "BMW" },
                new CarBrand { Id = 2, Name = "Mercedes" },
                new CarBrand { Id = 3, Name = "Audi" },
                new CarBrand { Id = 4, Name = "Toyota" },
                new CarBrand { Id = 5, Name = "Subaru" },
                new CarBrand { Id = 6, Name = "Mitsubishi" },
                new CarBrand { Id = 7, Name = "Ford" },
                new CarBrand { Id = 8, Name = "Daweoo" },
                new CarBrand { Id = 9, Name = "Lada" }
            );
            modelBuilder.Entity<CarColor>().HasData(
                new CarColor { Id = 1, Name = "Черный" },
                new CarColor { Id = 2, Name = "Белый" },
                new CarColor { Id = 3, Name = "Серый" },
                new CarColor { Id = 4, Name = "Красный" },
                new CarColor { Id = 5, Name = "Бордовый" },
                new CarColor { Id = 6, Name = "Зеленый" },
                new CarColor { Id = 7, Name = "Синий" },
                new CarColor { Id = 8, Name = "Фиолетовый" }
            );
        }
    }
}