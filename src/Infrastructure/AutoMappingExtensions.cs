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

        public static OrderDto SetOrderInfo(this Order order, User client) =>
            new()
            {
                OrderId = order.Id,
                StartCity = order.Route.StartCity,
                FinishCity = order.Route.FinishCity,
                Package = order.Package,
                CarType = order.CarType,
                IsSingle = order.IsSingle,
                Price = order.Price,
                StateName = order.State.Name,
                DeliveryDate = order.Delivery?.DeliveryDate ?? order.DeliveryDate,
                Location = order.Location,
                ClientName = client.Name,
                ClientSurname = client.Surname,
                ClientPhoneNumber = client.PhoneNumber,
                SecretCode = order.SecretCode
            };

        public static DeliveryDto SetDeliveryInfo(this Order order, User client, User driver) =>
            new()
            {
                OrderId = order.Id,
                StartCity = order.Route.StartCity,
                FinishCity = order.Route.FinishCity,
                Package = order.Package,
                CarType = order.CarType,
                IsSingle = order.IsSingle,
                Price = order.Price,
                StateName = order.State.Name,
                DeliveryDate = order.Delivery?.DeliveryDate ?? order.DeliveryDate,
                Location = order.Location,
                ClientName = client.Name,
                ClientSurname = client.Surname,
                ClientPhoneNumber = client.PhoneNumber,
                DriverPhoneNumber = driver.PhoneNumber,
                DriverName = driver.Name,
                DriverSurname = driver.Surname,
                SecretCode = order.SecretCode
            };
    }
}