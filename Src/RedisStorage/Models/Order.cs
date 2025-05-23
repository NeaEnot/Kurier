﻿using Kurier.Common.Enums;

namespace Kurier.RedisStorage.Models
{
    internal class Order
    {
        public Guid OrderId { get; set; }
        public Guid ClientId { get; set; }
        public string DepartureAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Weight { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
