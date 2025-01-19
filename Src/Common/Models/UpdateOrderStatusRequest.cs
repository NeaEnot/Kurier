using Kurier.Common.Enums;

namespace Kurier.Common.Models
{
    public class UpdateOrderStatusRequest
    {
        public Guid CourierTokenId { get; set; }
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
