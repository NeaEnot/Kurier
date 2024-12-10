using Kurier.Common.Enums;

namespace Kurier.Common.Models
{
    public class UpdateOrderStatusRequest
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
