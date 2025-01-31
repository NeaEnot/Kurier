namespace Kurier.Common.Models.Requests
{
    public class CreateOrderRequest
    {
        public Guid ClientTokenId { get; set; }
        public string DepartureAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Weight { get; set; }
    }
}
