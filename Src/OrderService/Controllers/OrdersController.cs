using Kurier.Common;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderStorage orderStorage;
        private readonly KafkaProducerHandler kafkaProducer;

        public OrdersController(IOrderStorage orderStorage, KafkaProducerHandler kafkaProducer)
        {
            this.orderStorage = orderStorage;
            this.kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            Guid id = await orderStorage.CreateOrder(request);

            OrderCreatedEvent evt = new OrderCreatedEvent
            {
                OrderId = id,
                Weight = request.Weight,
                DepartureAddress = request.DepartureAddress,
                DeliveryAddress = request.DeliveryAddress
            };

            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderCreatedEvents, id.ToString(), evt);

            return Ok(id);
        }

        [HttpPost]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await orderStorage.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            var order = await orderStorage.GetOrderById(orderId);

            if (order == null)
            {
                return NotFound();
            }

            UpdateOrderStatusRequest request = new UpdateOrderStatusRequest { OrderId = orderId, Status = OrderStatus.Canceled };
            await orderStorage.UpdateOrderStatus(request);

            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderCanceledEvents, orderId.ToString(), orderId);

            return Ok();
        }
    }
}
