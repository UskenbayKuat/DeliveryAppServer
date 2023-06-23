using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DataContextInterface;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.RejectedInterfaces;
using ApplicationCore.Interfaces.RouteInterfaces;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Interfaces.StateInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using ApplicationCore.Models.Values;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Config;
using Infrastructure.Services;
using Infrastructure.Services.BackgroundServices;
using Infrastructure.Services.ChatHubServices;
using Infrastructure.Services.ClientServices;
using Infrastructure.Services.DeliveryServices;
using Infrastructure.Services.DriverServices;
using Infrastructure.Services.RegisterServices;
using Infrastructure.Services.RejectedService;
using Infrastructure.Services.RouteServices;
using Infrastructure.Services.Shared;
using Infrastructure.Services.StateServices;
using Infrastructure.Services.TokenServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.HubNotify;
using Notification.Interfaces;

namespace PublicApi.Extensions
{
    public static class AppServicesExtensions
    {
        public static void GetServices(this IServiceCollection services, IConfiguration configuration)
        {
            //backgroundService
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

            //context
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            
            //services
            services.AddTransient<IDeliveryAppData<DriverAppDataInfo>, DriverAppDataService>();
            services.AddTransient<IDeliveryAppData<ClientAppDataInfo>, ClientAppDataService>();
            
            services.AddTransient<IOrderCommand, OrderCommand>();
            services.AddTransient<IOrderQuery, OrderQuery>();
            services.AddTransient<IDeliveryCommand, DeliveryCommand>();
            services.AddTransient<IDeliveryQuery, DeliveryQuery>(); 
      
            services.AddTransient<IRejected, RejectedService>();
            services.AddTransient<IState, StateService>();
            services.AddTransient<IRoute, RouteService>();
            services.AddTransient<IDriver, DriverService>();
            services.AddTransient<IClient, ClientService>();
            services.AddTransient<IChatHub, ChatHubService>();
            services.AddTransient<IValidation, ValidationService>();
            services.AddTransient<IGenerateToken, TokenService>();
            services.AddTransient<IRefreshToken, TokenService>();
            services.AddTransient<IRegistration, RegisterBySmsMockService>();
            services.AddTransient<IProceedRegistration, ProceedRegistrationService>();
            services.AddTransient<ICalculate, CalculateService>();
            services.AddTransient<INotify, Notify>();
            services.Configure<AuthOptions>(configuration.GetSection(AuthOptions.JwtSettings));
            services.ConfigureDbContextServices(configuration);
        }
        
        private static void ConfigureDbContextServices(this IServiceCollection services, IConfiguration configuration)
        {
            var useOnlyInMemoryDatabase = false;
            if (configuration["UseOnlyInMemoryDatabase"] != null)
            {
                useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]);
            }

            if (useOnlyInMemoryDatabase)
            {
                services.AddDbContext<AppDbContext>(c =>
                    c.UseInMemoryDatabase("AppDb"));
         
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseInMemoryDatabase("AppIdentityDb"));
            }
            else
            {
                services.AddDbContext<AppDbContext>(c =>
                    c.UseNpgsql(configuration.GetConnectionString("AppConnection")));

                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("IdentityConnection")));
            }
        }
        
    }
}