using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Kurier.RedisStorage
{
    public class RedisDeliveryStorage : IDeliveryStorage
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase db;

        public RedisDeliveryStorage(IConnectionMultiplexer redis)
        {
            this.redis = redis;
            db = this.redis.GetDatabase();
        }

        public async Task CreateDelivery(OrderDelivery request)
        {
            string key = GetDeliveryKey(request.OrderId);
            string courierKey = GetDeliveryCourierKey(request.CourierId);

            string value = JsonSerializer.Serialize(request);

            db.StringSetAsync(key, value);
            db.ListRightPushAsync(courierKey, value);
        }

        public async Task<List<OrderDelivery>> GetDeliveriesForCourier(Guid? courierId)
        {
            string courierKey = GetDeliveryCourierKey(courierId);

            RedisValue[] values = await db.ListRangeAsync(courierKey);

            List<OrderDelivery> deliveries =
                values
                .Where(value => !value.IsNullOrEmpty)
                .Select(value => JsonSerializer.Deserialize<OrderDelivery>(value))
                .ToList();

            return deliveries;
        }

        public async Task<OrderDelivery> GetDeliveryById(Guid orderId)
        {
            string key = GetDeliveryKey(orderId);
            RedisValue value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Delivery with ID {orderId} not found.");
            }

            OrderDelivery delivery = JsonSerializer.Deserialize<OrderDelivery>(value);
            return new OrderDelivery
            {
                OrderId = delivery.OrderId,
                CourierId = delivery.CourierId,
                Status = delivery.Status
            };
        }

        public async Task UpdateDelivery(OrderDelivery request)
        {
            string key = GetDeliveryKey(request.OrderId);
            RedisValue value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Delivery with ID {request.OrderId} not found.");
            }

            OrderDelivery delivery = JsonSerializer.Deserialize<OrderDelivery>(value);

            string courierKey = GetDeliveryCourierKey(delivery.CourierId);
            string newCourierKey = GetDeliveryCourierKey(request.CourierId);

            delivery.CourierId = request.CourierId;
            delivery.Status = request.Status;

            string updatedValue = JsonSerializer.Serialize(delivery);

            db.ListRemoveAsync(courierKey, value);

            if (delivery.Status == OrderStatus.Completed || delivery.Status == OrderStatus.Canceled)
            {
                db.KeyDeleteAsync(key);
            }
            else
            {
                db.StringSetAsync(key, updatedValue);
                db.ListRightPushAsync(newCourierKey, updatedValue);
            }
        }

        private string GetDeliveryKey(Guid orderId) => $"orderDelivery:{orderId}";
        private string GetDeliveryCourierKey(Guid? courierId) => $"courierDelivery:{(courierId == null ? "unassigned" : courierId)}";
    }
}
