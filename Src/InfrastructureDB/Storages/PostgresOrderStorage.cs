using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models.Entities;
using Kurier.Common.Models.Events;
using Kurier.Common.Models.Requests;
using Kurier.Common.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureDB.Storages
{
    public class PostgresOrderStorage : IOrderStorage
    {
        IRepository<Order> _repository;
        public PostgresOrderStorage(IRepository<Order> repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateOrder(CreateOrderInStorageRequest request)
        {
            Guid orderId = Guid.NewGuid();
            Order order = new()
            {
                Id = orderId,
                UserId = request.ClientId,
                DeliveryAddress = request.DeliveryAddress,
                DepartureAddress = request.DepartureAddress,
                Weight = request.Weight,
                Status = OrderStatus.Created,
                Created = DateTime.Now,
                LastUpdate = DateTime.Now
            };
            _repository.AddAsync(order);

            return orderId;
        }

        public async Task<GetOrderResponse> GetOrderById(Guid id)
        {
            var order = _repository.GetByIdAsync(id).Result;
            return new GetOrderResponse
            {
                OrderId = order.Id,
                ClientId = order.UserId,
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
            var order = _repository.GetByIdAsync(request.OrderId).Result;

            order.Status = request.Status;
            order.LastUpdate = DateTime.Now;

            _repository.UpdateAsync(order);

            return new OrderUpdatedEvent
            {
                OrderId = order.Id,
                ClientId = order.UserId,
                NewStatus = order.Status
            };
        }
    }
}
