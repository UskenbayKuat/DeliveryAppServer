using ApplicationCore;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.BackgroundTaskInterfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.ContextInterfaces;
using ApplicationCore.Interfaces.DeliveryInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.LocationInterfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Config;
using Infrastructure.Handlers;
using Infrastructure.Services;
using Infrastructure.Services.BackgroundServices;
using Infrastructure.Services.ChatHubServices;
using Infrastructure.Services.ClientServices;
using Infrastructure.Services.ContextBuilder;
using Infrastructure.Services.DeliveryServices;
using Infrastructure.Services.DriverServices;
using Infrastructure.Services.LocationServices;
using Infrastructure.Services.RegisterServices;
using Infrastructure.Services.Shared;
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
            services.AddTransient<IContext, ContextService>();
            services.AddTransient<IDeliveryContextBuilder, DeliveryContextBuilder>();
            services.AddTransient<IOrderContextBuilder, OrderContextBuilder>();
            services.AddTransient<IDriverContextBuilder, DriverContextBuilder>();
            services.AddTransient<ILocationDataContextBuilder, LocationDataContextBuilder>();

            
            services.AddTransient<IOrderCommand, OrderCommand>();
            services.AddTransient<IOrderQuery, OrderQuery>();
            services.AddTransient<IDeliveryCommand, DeliveryCommand>();
            services.AddTransient<IDeliveryQuery, DeliveryQuery>(); 
            
      
            services.AddTransient<IChatHub, ChatHubService>();
            services.AddTransient<ILocation, LocationService>();
            services.AddTransient<IValidation, ValidationService>();
            services.AddTransient<IGenerateToken, TokenService>();
            services.AddTransient<IRefreshToken, TokenService>();
            services.AddTransient<IRegistration, RegisterBySmsMockService>();
            services.AddTransient<IProceedRegistration, ProceedRegistrationService>();
            services.AddTransient<ICalculate, CalculateService>();
            services.AddTransient<ICar, CarService>();
            services.AddTransient<IDeliveryAppData<DriverAppDataInfo>, DriverAppDataService>();
            services.AddTransient<IDeliveryAppData<ClientAppDataInfo>, ClientAppDataService>();
            services.AddTransient<IUserData, UserDataService>();
            services.AddTransient<INotify, Notify>();
            services.AddTransient<IOrderHandler, OrderHandler>();
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