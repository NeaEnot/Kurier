using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

using Order = Kurier.RedisStorage.Models.Order;

namespace Kurier.RedisStorage
{
    public class RedisOrderStorage : IOrderStorage
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisOrderStorage(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task<Guid> CreateOrder(CreateOrderInStorageRequest request)
        {
            Guid orderId = Guid.NewGuid();
            Order order = new Order
            {
                OrderId = orderId,
                ClientId = request.ClientId,
                DeliveryAddress = request.DeliveryAddress,
                DepartureAddress = request.DepartureAddress,
                Weight = request.Weight,
                Status = OrderStatus.Created,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now
            };

            string key = GetOrderKey(orderId);
            string value = JsonSerializer.Serialize(order);

            await _db.StringSetAsync(key, value);

            return orderId;
        }

        public async Task<GetOrderResponse> GetOrderById(Guid id)
        {
            string key = GetOrderKey(id);
            RedisValue value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            Order order = JsonSerializer.Deserialize<Order>(value);
            return new GetOrderResponse
            {
                OrderId = order.OrderId,
                ClientId = order.ClientId,
                DeliveryAddress = order.DeliveryAddress,
                DepartureAddress = order.DepartureAddress,
                Weight = order.Weight,
                Status = order.Status,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now
            };
        }

        public async Task<OrderUpdatedEvent> UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            string key = GetOrderKey(request.OrderId);
            RedisValue value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
            }

            Order order = JsonSerializer.Deserialize<Order>(value);

            order.Status = request.Status;
            order.LastUpdate = DateTime.Now;

            string updatedValue = JsonSerializer.Serialize(order);
            await _db.StringSetAsync(key, updatedValue);

            return new OrderUpdatedEvent
            {
                OrderId = order.OrderId,
                ClientId = order.ClientId,
                NewStatus = order.Status
            };
        }

        private string GetOrderKey(Guid orderId) => $"order:{orderId}";
    }
}
