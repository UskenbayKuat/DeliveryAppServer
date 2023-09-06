namespace ApplicationCore.Models.Dtos.Deliveries
{
    public class QRCodeDto
    {
        public int OrderId { get; set; }
        public string SecretCode { get; set; }
        public string UserId { get; set; }
    }
}