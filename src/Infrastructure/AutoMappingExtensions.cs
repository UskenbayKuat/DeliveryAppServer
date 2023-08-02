using System;
using System.Collections.Generic;
using ApplicationCore.Entities;
using ApplicationCore.Entities.AppEntities;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Extensions;
using ApplicationCore.Models.Dtos;
using ApplicationCore.Models.Dtos.Deliveries;
using ApplicationCore.Models.Dtos.Histories;
using ApplicationCore.Models.Dtos.Orders;
using ApplicationCore.Models.Entities.Orders;
using ApplicationCore.Models.Enums;
using ApplicationCore.Models.Values;
using DeliveryDto = ApplicationCore.Models.Dtos.Deliveries.DeliveryDto;

namespace Infrastructure
{
    public static class AutoMappingExtensions
    {
        public static ActiveDeliveryDto MapToDeliveryDto(this Delivery delivery, List<OrderDto> orderDtoList)
        {
            var handOverCount = 0;
            var activeCount = 0;
            foreach(var o in orderDtoList)
            {
                if (o.StateName == GeneralState.PENDING_For_HAND_OVER.GetDisplayName())
                    handOverCount++;
                else if (o.StateName == GeneralState.RECEIVED_BY_DRIVER.GetDisplayName())
                    activeCount++;
            }
            return new()
            {
                StartCityName = delivery.Route.StartCity.Name,
                FinishCityName = delivery.Route.FinishCity.Name,
                DeliveryDate = delivery.DeliveryDate,
                HandOverCount = handOverCount,
                ActiveCount = activeCount,
                IsStartVisible = delivery.State.StateValue == GeneralState.INPROGRESS,
                OrderDtoList = orderDtoList,
                StateName = delivery.State.Name
            };
        }
        public static HistoryDeliveryDto MapToHistoryDto(this Delivery delivery, List<OrderDto> orderDtoList)
        {
            return new()
            {
                StartCityName = delivery.Route.StartCity.Name,
                FinishCityName = delivery.Route.FinishCity.Name,
                StartDate = delivery.DeliveryDate,
                FinishDate = delivery.CompletionDate ?? delivery.CancellationDate.Value,
                OrderDtoList = orderDtoList,
                StateName = delivery.State.Name,
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
                DeliveryDate = order.DeliveryDate,
                ClientName = client.Name,
                ClientSurname = client.Surname,
                ClientPhoneNumber = client.PhoneNumber,
                Latitude = order.Location.Latitude,
                Longitude = order.Location.Longitude,
                AddressFrom = order.AddressFrom,
                AddressTo = order.AddressTo,
                Description = order.Description,
                IsConfirm = order.State.StateValue == GeneralState.ON_REVIEW
            };

        public static DeliveryDto GetDeliveryDto(this Order order, User client, User driver, StateHistoryDto stateHistoryDto) =>
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
                DriverPhoneNumber = driver?.PhoneNumber,
                DriverName = driver?.Name,
                DriverSurname = driver?.Surname,
                CarNumber = order.Delivery?.Driver.Car.CarNumber,
                DeliveryState = order.State.StateValue.GetDisplayName(),
                SecretCode = order.SecretCode,
                DeliveryDate = order.DeliveryDate,
                StateHistoryDto = stateHistoryDto,
                CreateDate = order.CreatedDate
            };

        public static StateHistoryDto GetStateHistoryDto(this List<OrderStateHistory> stateHistoryList)
        {
            var dto = new StateHistoryDto();
            foreach (var item in stateHistoryList)
            {
                switch (item.State.StateValue)
                {
                    case GeneralState.PENDING_For_HAND_OVER:
                        dto.PendingForHandOver = item.CreatedDate.ToString("dd.MM.yyyy, hh\\:mm");
                        break;
                    case GeneralState.RECEIVED_BY_DRIVER:
                        dto.ReceivedByDriver = item.CreatedDate.ToString("dd.MM.yyyy, hh\\:mm");
                        break;
                    case GeneralState.DONE:
                        dto.Done = item.CreatedDate.ToString("dd.MM.yyyy, hh\\:mm");
                        break;
                }
            }
            return dto;
        }

    }
}