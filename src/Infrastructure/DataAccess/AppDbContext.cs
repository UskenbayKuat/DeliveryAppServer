using ApplicationCore.Entities.AppEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarType> CarTypes { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Kit> Kits { get; set; }
        public DbSet<DriverKit> DriversKits { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<RouteTrip> RouteTrips { get; set; }
        public DbSet<ClientPackage> ClientPackages { get; set; }
        public DbSet<Location> Locations { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientPackage>().Property(c => c.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.OrderCost).HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<CarType>().HasData(
                new {Id = 1, Name = "Седан"},
                new {Id = 2, Name = "Минивэн"},
                new {Id = 3, Name = "Фургон"});
            
            modelBuilder.Entity<City>().HasData(
                new {Id = 1, Name = "Алматы"},
                new {Id = 2, Name = "Шымкент"},
                new {Id = 3, Name = "Нур-Султан"});
            
            modelBuilder.Entity<Kit>().HasData(
                new {Id = 1, Name = "Light", Quantity = 5, IsUnlimited = false},
                new {Id = 2, Name = "Standard ", Quantity = 10, IsUnlimited = false},
                new {Id = 3, Name = "Premium", Quantity = 15, IsUnlimited = false},
                new {Id = 4, Name = "Unlimited", Quantity = 999999, IsUnlimited = true});

            modelBuilder.Entity<Route>().HasData(
                new {Id = 1, StartCityId = 1, FinishCityId = 2, Price = (decimal)1000},
                new {Id = 2, StartCityId = 1, FinishCityId = 3, Price = (decimal)2000},
                new {Id = 3, StartCityId = 2, FinishCityId = 1, Price = (decimal)1000},
                new {Id = 4, StartCityId = 3, FinishCityId = 1, Price = (decimal)2000},
                new {Id = 5, StartCityId = 2, FinishCityId = 3, Price = (decimal)2000},
                new {Id = 6, StartCityId = 3, FinishCityId = 2, Price = (decimal)2000}
            );
        }
    }
}