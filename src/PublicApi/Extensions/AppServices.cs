using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using Infrastructure.Services;
using Infrastructure.Services.ClientService;
using Infrastructure.Services.DriverService;
using Infrastructure.Services.RegisterServices;
using Infrastructure.Services.Shared;
using Infrastructure.Services.TokenServices;
using Microsoft.Extensions.DependencyInjection;

namespace PublicApi.Extensions
{
    public static class AppServices
    {
        public static void GetServices(this IServiceCollection services)
        {
            services.AddTransient<IValidation, ValidationService>();
            services.AddTransient<IGenerateToken, TokenService>();
            services.AddTransient<IRefreshToken, TokenService>();
            services.AddTransient<IRouteTrip, RouteTripService>();
            services.AddTransient<IRegistration, RegisterBySmsMockService>();
            services.AddTransient<IProceedRegistration, ProceedRegistrationService>();
            services.AddTransient<ICalculate, CalculatePriceService>();
            services.AddTransient<IClientPackage, ClientPackageService>();
            services.AddTransient<ICreateCar, CreateCarService>();
            services.AddTransient<IDeliveryAppData<DriverAppDataInfo>, DriverAppDataService>();
            services.AddTransient<IDeliveryAppData<ClientAppDataInfo>, ClientAppDataService>();
        }
    }
}