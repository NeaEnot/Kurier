using Kurier.Common.Enums;

namespace Kurier.Common.Models
{
    public class OrderDelivery
    {
        public Guid OrderId { get; set; }
        public Guid? CourierId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
