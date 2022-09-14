using ApplicationCore.Interfaces;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            
            services.AddTransient<IValidation, ValidationMobileData>();
            services.AddTransient<IRouteTrip, RouteTripService>();
            services.AddTransient<IRegistration, RegisterBySmsMock>();
            services.AddTransient<IProceedRegistration, ProceedRegistrationUser>();
            services.AddTransient<IGetCities, SendCitiesList>();
            services.AddTransient<IGetCarTypes, SendCarTypesList>();
            services.AddTransient<IGetKits, SendKitsList>();
            services.AddTransient<IGetRouteTrip, SendRouteTrip>();
            services.AddTransient<ICalculate, CalculatePrice>();
            services.AddTransient<IConfirmOrder, ConfirmClientPackage>();
            services.AddTransient<ICreateCar, CreateCarService>();
            services.AddControllers(options =>
            {
                options.UseNamespaceRouteToken();
            });
            
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<Notification>("/notification");
            });
        }
    }
}