using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Clients;
using ApplicationCore.Interfaces.Drivers;
using ApplicationCore.Interfaces.Register;
using ApplicationCore.Interfaces.Shared;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Shared;
using ApplicationCore.Models.Values;
using Infrastructure.Context.DataAccess;
using Infrastructure.Context.Identity;
using Infrastructure.Config;
using Infrastructure.Services;
using Infrastructure.Services.Clients;
using Infrastructure.Services.Drivers;
using Infrastructure.Services.Register;
using Infrastructure.Services.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.Services;

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
            services.AddTransient<IDeliveryAppData<DriverAppDataDto>, DriverAppDataService>();
            services.AddTransient<IDeliveryAppData<ClientAppDataDto>, ClientAppDataService>();
            
            services.AddTransient<IOrderCommand, OrderCommand>();
            services.AddTransient<IOrderQuery, OrderQuery>();
            services.AddTransient<IDeliveryCommand, DeliveryCommand>();
            services.AddTransient<IDeliveryQuery, DeliveryQuery>(); 
            services.AddTransient<IOrderStateHistory, OrderStateHistoryService>(); 
      
            services.AddTransient<IRejectedOrder, RejectedOrderService>();
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
            services.AddTransient<INotify, NotifyService>();
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