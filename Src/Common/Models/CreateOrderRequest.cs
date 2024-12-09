namespace Kurier.Common.Models
{
    public class CreateOrderRequest
    {
        public Guid ClientId { get; set; }
        public string DepartureAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Weight { get; set; }
    }
}
