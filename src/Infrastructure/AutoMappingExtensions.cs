using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Extensions;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Models.Values;
using DeliveryDto = ApplicationCore.Models.Dtos.Deliveries.DeliveryDto;

namespace Infrastructure
{
    public static class AutoMappingExtensions
    {
        public static IsActiveDeliveryDto GetDeliveryDto(this Delivery delivery, List<OrderDto> orderInfos)
        {
            var handOverCount = 0;
            var activeCount = 0;
            orderInfos.ForEach(o =>
            {
                if (o.StateName == GeneralState.PendingForHandOver.GetDisplayName())
                    handOverCount++;
                else if (o.StateName == GeneralState.ReceivedByDriver.GetDisplayName())
                    activeCount++;
            });
            return new()
            {
                StartCityName = delivery.Route.StartCity.Name,
                FinishCityName = delivery.Route.FinishCity.Name,
                DeliveryDate = delivery.DeliveryDate,
                HandOverCount = handOverCount,
                ActiveCount = activeCount,
                OrderCount = activeCount + handOverCount,
                IsStartVisible = delivery.State.StateValue == GeneralState.InProgress,
                OrderInfos = orderInfos
            };
        }

        public static OrderDto GetOrderDto(this Order order, User client) =>
            new()
            {
                OrderId = order.Id,
                StartCityName = order.Route.StartCity.Name,
                FinishCityName = order.Route.FinishCity.Name,
                Package = order.Package,
                CarTypeName = order.CarType?.Name,
                IsSingle = order.IsSingle,
                Price = order.Price,
                StateName = order.State.Name,
                DeliveryDate = order.Delivery?.DeliveryDate ?? order.DeliveryDate,
                ClientName = client.Name,
                ClientSurname = client.Surname,
                ClientPhoneNumber = client.PhoneNumber,
                Latitude = order.Location.Latitude,
                Longitude = order.Location.Longitude,
                AddressFrom = order.AddressFrom,
                AddressTo = order.AddressTo,
                Description = order.Description
            };

        public static DeliveryDto GetDeliveryDto(this Order order, User client, User driver) =>
            new()
            {  
                StartCityName = order.Route.StartCity.Name,
                FinishCityName = order.Route.FinishCity.Name,
                AddressFrom = order.AddressFrom,
                AddressTo = order.AddressTo,
                Description = order.Description,
                OrderId = order.Id,
                Package = order.Package,
                IsSingle = order.IsSingle,
                Price = order.Price,
                ClientName = client.Name,
                ClientSurname = client.Surname,
                ClientPhoneNumber = client.PhoneNumber,
                DriverPhoneNumber = driver.PhoneNumber,
                DriverName = driver.Name,
                DriverSurname = driver.Surname,
                CarNumber = order.Delivery.Driver.Car.CarNumber,
                DeliveryState = order.Delivery.State.StateValue.GetDisplayName(),
                SecretCode = order.SecretCode,
            };
    }
}