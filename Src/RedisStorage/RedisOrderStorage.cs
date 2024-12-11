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

        public async Task<Guid> CreateOrder(CreateOrderRequest request)
        {
            var orderId = Guid.NewGuid();
            var order = new Order
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

            var key = GetOrderKey(orderId);
            var value = JsonSerializer.Serialize(order);

            await _db.StringSetAsync(key, value);

            return orderId;
        }

        public async Task<GetOrderResponse> GetOrderById(Guid id)
        {
            var key = GetOrderKey(id);
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Order with ID {id} not found.");
            }

            var order = JsonSerializer.Deserialize<Order>(value);
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

        public async Task UpdateOrderStatus(UpdateOrderStatusRequest request)
        {
            var key = GetOrderKey(request.OrderId);
            var value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
            }

            var order = JsonSerializer.Deserialize<Order>(value);

            order.Status = request.Status;
            order.LastUpdate = DateTime.Now;

            var updatedValue = JsonSerializer.Serialize(order);
            await _db.StringSetAsync(key, updatedValue);
        }

        private string GetOrderKey(Guid orderId) => $"order:{orderId}";
    }
}
