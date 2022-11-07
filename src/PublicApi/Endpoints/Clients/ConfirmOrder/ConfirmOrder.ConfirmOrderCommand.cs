using System;
using ApplicationCore.Entities.AppEntities;

namespace PublicApi.Endpoints.Clients.ConfirmOrder
{
    public class ConfirmOrderCommand
    {
        public int ClientId { get; set; }
        public int StartCityId { get; set; }
        public int FinishCityId { get; set; }
        public Package Package { get; set; }
        public DateTime DateTime { get; set; }
        public int CarTypeId { get; set; }
        public bool IsSingle { get; set; }
        public double Price { get; set; }
    }
}