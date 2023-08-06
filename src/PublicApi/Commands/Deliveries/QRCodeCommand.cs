
using MediatR;

namespace PublicApi.Commands.Deliveries
{
    public class QRCodeCommand : IRequest
    {
        public int OrderId { get; set; }
        public string SecretCode { get; set; }
        public string UserId { get; private set; }
        public QRCodeCommand SetUserId(string userId)
        {
            UserId = userId;
            return this;
        }
    }
}