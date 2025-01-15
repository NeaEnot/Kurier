using Kurier.Common;
using Kurier.Common.Interfaces;
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
        private IAuthTokenStorage authTokenStorage;

        public DeliveryController(KafkaProducerHandler kafkaProducer, IAuthTokenStorage authTokenStorage)
        {
            this.kafkaProducer = kafkaProducer;
            this.authTokenStorage = authTokenStorage;
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
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request, [FromBody] Guid courierTokenId)
        {
            UserAuthToken token = await authTokenStorage.GetToken(courierTokenId);

            // STUB
            // Проверяем, что изменяет статус тот курьер, который назначен на заказ

            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderStatus, request.OrderId.ToString(), request);

            return Ok();
        }
    }
}
