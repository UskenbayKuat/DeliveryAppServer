using System;

namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class QRCodeDto
    {
        public Guid OrderId { get; set; }
        public string SecretCode { get; set; }
        public Guid UserId { get; set; }
    }
}