using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Kurier.OrderService.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderStorage orderStorage;
        private readonly KafkaProducerHandler kafkaProducer;

        public OrdersController(IOrderStorage orderStorage)
        {
            this.orderStorage = orderStorage;
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

            await kafkaProducer.PublishEventAsync("order-events", id.ToString(), evt);

            return Ok(id);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await orderStorage.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
    }
}
