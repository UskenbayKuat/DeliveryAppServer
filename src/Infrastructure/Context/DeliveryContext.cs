using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Locations;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Entities.AppEntities.UIMessages;
using ApplicationCore.Models.Entities;
using ApplicationCore.Models.Entities.Cars;
using ApplicationCore.Models.Entities.Locations;
using ApplicationCore.Models.Entities.Orders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context
{
    public sealed class DeliveryContext : DbContext
    {
        //users
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        //logger
        public DbSet<MobileLogger> MobileLoggers { get; set; }

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

        //locations
        public DbSet<Location> Locations { get; set; }

        public DbSet<LocationData> LocationData { get; set; }

        //orders
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<RejectedOrder> RejectedOrders { get; set; }
        public DbSet<OrderStateHistory> OrderStateHistories { get; set; }
        public DbSet<State> States { get; set; }

        //kits
        public DbSet<Kit> Kits { get; set; }
        public DbSet<DriverKit> DriversKits { get; set; }

        //chat hub
        public DbSet<ChatHub> ChatHubs { get; set; }
        public DbSet<MessageForUser> MessagesForUser { get; set; }


        public DeliveryContext(DbContextOptions<DeliveryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(c => c.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Package>().Property(c => c.Height).HasPrecision(18, 2);
            modelBuilder.Entity<Package>().Property(c => c.Length).HasPrecision(18, 2);
            modelBuilder.Entity<Package>().Property(c => c.Weight).HasPrecision(18, 2);
            modelBuilder.Entity<Package>().Property(c => c.Width).HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}