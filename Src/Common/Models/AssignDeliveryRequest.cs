namespace Kurier.Common.Models
{
    public class AssignDeliveryRequest
    {
        public Guid OrderId { get; set; }
        public Guid CourierId { get; set; }
    }
}
