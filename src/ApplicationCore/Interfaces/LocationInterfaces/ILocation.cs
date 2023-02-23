using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Entities.AppEntities.Orders;
using ApplicationCore.Entities.Values;

namespace ApplicationCore.Interfaces.LocationInterfaces
{
    public interface ILocation
    {
        public Task<LocationInfo> SendDriverLocationAsync(LocationInfo locationInfo, int deliveryId);
    }
}