using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedAsync(AppIdentityDbContext appIdentityDbContext)
        {
            if (appIdentityDbContext.Database.IsNpgsql())
            {
                await appIdentityDbContext.Database.MigrateAsync();
            }
        }
    }
}