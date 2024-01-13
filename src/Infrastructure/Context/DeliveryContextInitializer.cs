using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Cars;
using ApplicationCore.Entities.AppEntities.Routes;
using ApplicationCore.Extensions;
using ApplicationCore.Models.Entities;
using ApplicationCore.Models.Entities.Dictionaries;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;

namespace Infrastructure.Context
{
    public class DeliveryContextInitializer
    {
        public static void SeedAsync(DeliveryContext context, ILogger<DeliveryContextInitializer> logger) =>
            Policy.Handle<PostgresException>()
                .WaitAndRetry(new[]
                    {
                        TimeSpan.FromSeconds(4),
                        TimeSpan.FromSeconds(3),
                        TimeSpan.FromSeconds(2)
                    },
                    (ex, span) =>
                    {
                        logger.LogWarning("Failed! Waiting {0}", span);
                        logger.LogWarning("Error was {0}", ex.GetType().Name);
                        logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(DeliveryContext));
                    })
                .Execute(() => InvokeSeeder(context).Wait());


        private static async Task InvokeSeeder(DeliveryContext context)
        {
            if (!await context.Cities.AnyAsync())
            {
                await context.Cities.AddRangeAsync(GetPreconfiguredCities());
                await context.SaveChangesAsync();
            }

            if (!await context.RoutePrice.AnyAsync())
            {
                await context.RoutePrice.AddRangeAsync(GetPreconfiguredRoutePrice(context));
                await context.SaveChangesAsync();
            }

            if (!await context.CarTypes.AnyAsync())
            {
                await context.CarTypes.AddRangeAsync(GetPreconfiguredCarTypes());
            }

            if (!await context.CarBrands.AnyAsync())
            {
                await context.CarBrands.AddRangeAsync(GetPreconfiguredCarBrands());
            }

            if (!await context.CarColors.AnyAsync())
            {
                await context.CarColors.AddRangeAsync(GetPreconfiguredCarColors());
            }

            if (!await context.Kits.AnyAsync())
            {
                await context.Kits.AddRangeAsync(GetPreconfiguredKits());
            }
            if (!await context.States.AnyAsync())
            {
                await context.States.AddRangeAsync(GetPreconfiguredStates());
            }
            if (!await context.Roles.AnyAsync())
            {
                await context.Roles.AddRangeAsync(GetRoles());
                await context.SaveChangesAsync();
            }
            if (!await context.UserRoles.AnyAsync())
            {
                await context.UserRoles.AddRangeAsync(GetUserRoles(context));
            }
            if (!await context.DicSmsLogs.AnyAsync())
            {
                await context.DicSmsLogs.AddAsync(GetDicSmsLog(context));
            }
            await context.SaveChangesAsync();
        }

        private static DicSmsLog GetDicSmsLog(DeliveryContext context)
        {
            return new DicSmsLog(
                minuteLife: 1,
                tryCount: 3,
                inputCount: 3,
                message: "Добро пожаловать! Для завершения процесса подтверждения, введите следующий код: {0}",
                kazInfoErrorMessage: "Извините за неудобства! В настоящее время отправка сообщений временно недоступна",
                tryCountMessage: "Пожалуйста, обращайтесь в службу поддержки",
                inputCountMessage: "Превышено количество попыток ввода с неверным кодам",
                codeErrorMessage: "Неправильный код",
                lifeTimeErrorMessage: "Пожалуйста, повторно введите свой номер");
        }

        private static UserRole[] GetUserRoles(DeliveryContext context)
        {
            var role = context.Roles.First(x => x.RoleValue == RoleEnum.Admin);
            var userRoles = new UserRole()
            {
                Role = role,
                User = new("Admin", "-", "admin@admin.com", "Админ", true)
            };
            return new UserRole[] { userRoles };
        }

        private static List<Role> GetRoles()
        {
            var roles = new List<Role>();
            foreach (var item in Enum.GetValues(typeof(RoleEnum)).Cast<RoleEnum>().ToList())
            {
                roles.Add(new(item));
            }
            return roles;
        }

        private static IEnumerable<City> GetPreconfiguredCities() =>
            new List<City>
            {
                new("Алматы"),
                new("Шымкент"),
                new("Астана")
            };

        private static IEnumerable<RoutePrice> GetPreconfiguredRoutePrice(DeliveryContext context)
        {
            var cityFirstId = context.Cities.First(x => x.Name == "Алматы").Id;
            var citySecondId = context.Cities.First(x => x.Name == "Шымкент").Id;
            var cityThirdId = context.Cities.First(x => x.Name == "Астана").Id;

            return new List<RoutePrice>()
            {
                new(1000) { Route = new(cityFirstId, citySecondId)},
                new(2000){ Route = new(cityFirstId, cityThirdId) },
                new(1000){ Route = new(citySecondId, cityFirstId)},
                new(2000){ Route = new(cityThirdId, cityFirstId)},
                new(2000){ Route = new(citySecondId, cityThirdId)},
                new(2000){ Route = new(cityThirdId, citySecondId)},
            };
        }

        private static IEnumerable<CarType> GetPreconfiguredCarTypes()
        {
            var carTypeList = new List<CarType>();
            foreach (var item in Enum.GetValues(typeof(CarTypeEnum)).Cast<CarTypeEnum>().ToList())
            {
                carTypeList.Add(new(item));
            }
            return carTypeList;
        }

        private static IEnumerable<CarBrand> GetPreconfiguredCarBrands()
        {
            var carBrandList = new List<CarBrand>();
            foreach (var item in Enum.GetValues(typeof(CarBrandEnum)).Cast<CarBrandEnum>().ToList())
            {
                carBrandList.Add(new(item));
            }
            return carBrandList;
        }

        private static IEnumerable<CarColor> GetPreconfiguredCarColors()
        {
            var colorList = new List<CarColor>();
            foreach (var item in Enum.GetValues(typeof(ColorEnum)).Cast<ColorEnum>().ToList())
            {
                colorList.Add(new(item));
            }
            return colorList;
        }

        private static IEnumerable<Kit> GetPreconfiguredKits() =>
            new List<Kit>
            {
                new(KitEnum.Light, 5, 4000),
                new(KitEnum.Standard, 10, 7000),
                new(KitEnum.Premium, 15, 9000),
                new(KitEnum.Unlimited, 1000,11000, true),
            };
        private static IEnumerable<State> GetPreconfiguredStates()
        {
            var stateList = new List<State>();
            foreach (var item in Enum.GetValues(typeof(GeneralState)).Cast<GeneralState>().ToList())
            {
                stateList.Add(new(item, item.GetDisplayName(), item.ToString()));
            }
            return stateList;
        }
    }
}