using ApplicationCore;
using ApplicationCore.Entities.Values;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.HubInterfaces;
using ApplicationCore.Interfaces.OrderInterfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using Infrastructure;
using Infrastructure.AppData.DataAccess;
using Infrastructure.AppData.Identity;
using Infrastructure.Config;
using Infrastructure.Services;
using Infrastructure.Services.ClientService;
using Infrastructure.Services.DriverService;
using Infrastructure.Services.HubServices;
using Infrastructure.Services.OrderServices;
using Infrastructure.Services.RegisterServices;
using Infrastructure.Services.Shared;
using Infrastructure.Services.TokenServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PublicApi.Extensions
{
    public static class AppServicesExtensions
    {
        public static void GetServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<OnReviewData>();
            services.AddTransient<IClientPackage, ClientPackageService>();
            services.AddTransient<IDriver, DriverService>();
            services.AddTransient<IHubConnect, HubConnectService>();
            services.AddTransient<IOrder, OrderService>();
            services.AddTransient<IValidation, ValidationService>();
            services.AddTransient<IGenerateToken, TokenService>();
            services.AddTransient<IRefreshToken, TokenService>();
            services.AddTransient<IRouteTrip, RouteTripService>();
            services.AddTransient<IRegistration, RegisterBySmsMockService>();
            services.AddTransient<IProceedRegistration, ProceedRegistrationService>();
            services.AddTransient<ICalculate, CalculatePriceService>();
            services.AddTransient<ICreateCar, CreateCarService>();
            services.AddTransient<IDeliveryAppData<DriverAppDataInfo>, DriverAppDataService>();
            services.AddTransient<IDeliveryAppData<ClientAppDataInfo>, ClientAppDataService>();
            services.AddTransient<IUserData, UserDataService>();
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