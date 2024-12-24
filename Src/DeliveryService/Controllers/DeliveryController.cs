using Kurier.Common;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DeliveryController : ControllerBase
    {
        private readonly KafkaProducerHandler kafkaProducer;

        public DeliveryController(KafkaProducerHandler kafkaProducer)
        {
            this.kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        public async Task<IActionResult> AssignCourierForDelivery([FromBody] AssignDeliveryRequest request)
        {
            // STUB
            // Записываем в Redis что курьер связан с заказом
            // Может быть отправить через kafka сообщение клиенту о назначении курьера

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request)
        {
            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderStatus, request.OrderId.ToString(), request);

            return Ok();
        }
    }
}
