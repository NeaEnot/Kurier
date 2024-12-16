using Kurier.Common.Enums;

namespace Kurier.Common.Models
{
    public class OrderUpdatedEvent
    {
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public OrderStatus NewStatus { get; set; }
    }
}
