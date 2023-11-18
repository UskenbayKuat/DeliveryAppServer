using System.Threading.Tasks;
using Infrastructure.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using PublicApi.Extensions;

namespace PublicApi
{
    public static class Program
    {
        public static async Task Main(string[] args) =>
            await CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<DeliveryContext>((context, provider) =>
                {
                    var logger = provider.GetService<ILogger<DeliveryContextInitializer>>();
                    DeliveryContextInitializer.SeedAsync(context, logger);
                })
                .RunAsync();

        private static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
    }
}