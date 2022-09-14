using System.Threading.Tasks;
using Infrastructure.DataAccess;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PublicApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            using (var scope = host.Services.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                
                var loggerFactory = scopedProvider.GetRequiredService<ILoggerFactory>();
                var appDbContext = scopedProvider.GetRequiredService<AppDbContext>();
                await AppDbContextSeed.SeedAsync(appDbContext, loggerFactory);
                    
                var appIdentityDbContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
                await AppIdentityDbContextSeed.SeedAsync(appIdentityDbContext);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}