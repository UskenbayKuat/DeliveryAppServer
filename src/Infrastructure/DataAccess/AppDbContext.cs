using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.AppEntities.UIMessages;
using ApplicationCore.Enums;
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
        public DbSet<RejectedClientPackage>  RejectedClientPackages { get; set; }

        //kits
        public DbSet<Kit> Kits { get; set; }
        public DbSet<DriverKit> DriversKits { get; set; }

        //chat hub
        public DbSet<ChatHub> ChatHubs { get; set; }
        public DbSet<MessageForUser> MessagesForUser { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientPackage>().Property(c => c.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.OrderCost).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
            
            #region Routes
            
            modelBuilder.Entity<City>().HasData(
                new City(1, "Алматы"),
                new City(2, "Шымкент"),
                new City(3, "Астана")
            );
            modelBuilder.Entity<Route>().HasData(
                new Route(1, 1, 2),
                new Route(2, 1, 3),
                new Route(3, 2, 1),
                new Route(4, 3, 1),
                new Route(5, 2, 3),
                new Route(6, 3, 2)
            );
            modelBuilder.Entity<RoutePrice>().HasData(
                new RoutePrice(1, 1, 1000),
                new RoutePrice(2, 2, 2000),
                new RoutePrice(3, 3, 1000),
                new RoutePrice(4, 4, 2000),
                new RoutePrice(5, 5, 2000),
                new RoutePrice(6, 6, 2000)
            );
            
            #endregion
            
            #region Cars
            
            modelBuilder.Entity<CarType>().HasData(
                new CarType(1, "Седан"),
                new CarType(2, "Минивэн"),
                new CarType(3, "Фургон")
            );
            
            modelBuilder.Entity<CarBrand>().HasData(
                new CarBrand(1, "BMW"),
                new CarBrand(2, "Mercedes"),
                new CarBrand(3, "Audi"),
                new CarBrand(4, "Toyota"),
                new CarBrand(5, "Subaru"),
                new CarBrand(6, "Mitsubishi"),
                new CarBrand(7, "Ford"),
                new CarBrand(8, "Daweoo"),
                new CarBrand(9, "Lada")
            );
            modelBuilder.Entity<CarColor>().HasData(
                new CarColor(1, "Черный"),
                new CarColor(2, "Белый"),
                new CarColor(3, "Серый"),
                new CarColor(4, "Красный"),
                new CarColor(5, "Бордовый"),
                new CarColor(6, "Зеленый"),
                new CarColor(7, "Синий"),
                new CarColor(8, "Фиолетовый")
            );
            
            #endregion
            
            modelBuilder.Entity<Kit>().HasData(
                new Kit(1, "Light", 5, false),
                new Kit(2, "Standard ", 10, false),
                new Kit(3, "Premium", 15, false),
                new Kit(4, "Unlimited", 999999, true)
            );

            modelBuilder.Entity<Status>().HasData(
                new Status(1, State.New.ToString()),
                new Status(2, State.InProgress.ToString()),
                new Status(3, State.Done.ToString()),
                new Status(4, State.Delayed.ToString()),
                new Status(5, State.Canceled.ToString())
            );
        }
    }
}