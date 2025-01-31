namespace Kurier.Common.Models.Requests
{
    public class CreateOrderInStorageRequest
    {
        public Guid ClientId { get; set; }
        public string DepartureAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Weight { get; set; }
    }
}
