
namespace PublicApi.Commands.Deliveries
{
    public class ConfirmHandOverCommand
    {
        public int OrderId { get; set; }
        public string SecretCode { get; set; }
    }
}