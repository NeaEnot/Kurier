namespace Kurier.Common.Models.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }
        public decimal Weight { get; set; }
        public string DepartureAddress { get; set; }
        public string DeliveryAddress { get; set; }
    }
}
