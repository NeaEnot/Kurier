namespace Kurier.Common.Models.Requests
{
    public class AssignDeliveryRequest
    {
        public Guid OrderId { get; set; }
        public Guid CourierId { get; set; }
    }
}
