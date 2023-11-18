
using MediatR;
using System;

namespace PublicApi.Commands.Deliveries.Models
{
    public class QRCodeCommand : IRequest
    {
        public Guid OrderId { get; set; }
        public string SecretCode { get; set; }
        public Guid UserId { get; private set; }
        public QRCodeCommand SetUserId(string userId)
        {
            UserId = Guid.Parse(userId);
            return this;
        }
    }
}