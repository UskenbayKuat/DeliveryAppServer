using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.AppEntities.UIMessages;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.AppData.DataAccess
{
    public sealed class AppDbContext : DbContext
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
        public DbSet<RoutePrice> RoutePrice { get; set; }

        public DbSet<RouteTrip> RouteTrips { get; set; }

        //locations
        public DbSet<Location> Locations { get; set; }

        public DbSet<LocationDate> LocationDate { get; set; }

        //orders
        public DbSet<Order> Orders { get; set; }
        public DbSet<ClientPackage> ClientPackages { get; set; }
        public DbSet<WaitingClientPackage> WaitingClientPackages { get; set; }
        public DbSet<Package> Packages { get; set; }
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
        }
    }
}