using System;
using System.Text;
using ApplicationCore.Entities.ApiEntities;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.ClientInterfaces;
using ApplicationCore.Interfaces.DriverInterfaces;
using ApplicationCore.Interfaces.RegisterInterfaces;
using ApplicationCore.Interfaces.SharedInterfaces;
using ApplicationCore.Interfaces.TokenInterfaces;
using Infrastructure;
using Infrastructure.Config;
using Infrastructure.Services;
using Infrastructure.Services.ClientService;
using Infrastructure.Services.DriverService;
using Infrastructure.Services.RegisterServices;
using Infrastructure.Services.Shared;
using Infrastructure.Services.TokenServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PublicApi.Hub;

namespace PublicApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Dependencies.ConfigureServices(Configuration, services);

             //подключение jwt

             var key = Encoding.ASCII.GetBytes(Configuration["JwtSettings:SecretKey"]);
             services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.TokenValidationParameters = AuthOptions.ValidationParameters(key, true);
                });
             
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
            services.AddControllers(options => { options.UseNamespaceRouteToken(); });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressInferBindingSourcesForParameters = true;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PublicApi", Version = "v1" });
            });
            services.AddSignalR();
            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<Notification>("/notification");
            });
        }
    }
}