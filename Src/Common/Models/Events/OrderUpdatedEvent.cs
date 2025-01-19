using Kurier.Common.Enums;

namespace Kurier.Common.Models.Events
{
    public class OrderUpdatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public OrderStatus NewStatus { get; set; }
    }
}
