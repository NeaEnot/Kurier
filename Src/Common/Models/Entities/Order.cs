using Kurier.Common.Enums;

namespace Kurier.Common.Models.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string DepartureAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Weight { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
    }
}
