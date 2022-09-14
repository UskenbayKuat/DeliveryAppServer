using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataAccess
{
    public class AppDbContextSeed
    {
        public static async Task SeedAsync(AppDbContext appDbContext,
            ILoggerFactory logger,
            int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (appDbContext.Database.IsNpgsql())
                {
                    appDbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;

                retryForAvailability++;
            
                // logger.LogError(ex.Message);
                await SeedAsync(appDbContext, logger, retryForAvailability);
                throw;
            }
        }
    }
}