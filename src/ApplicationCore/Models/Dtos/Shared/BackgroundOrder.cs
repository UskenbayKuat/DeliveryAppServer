using System;

namespace ApplicationCore.Models.Dtos.Shared
{
    public class BackgroundOrder
    {
        private const int WaitingPeriodMinute = 30;
        public Guid OrderId { get; private set; }
        public Guid DeliveryId { get; private set; }
        public DateTime WaitingPeriodTime { get; }

        public BackgroundOrder(Guid orderId, Guid deliveryId)
        {
            OrderId = orderId;
            DeliveryId = deliveryId;
            WaitingPeriodTime = DateTime.UtcNow.AddSeconds(WaitingPeriodMinute);
        }
    }
}