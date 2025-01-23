using Kurier.Common;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Kurier.Common.Models.Requests;
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
        [RequireAuthAndPermissions(UserPermissions.AssignSelfToDelivery | UserPermissions.AssignOthersToDelivery)]
        public async Task<IActionResult> AssignCourierForDelivery([FromBody] AssignDeliveryRequest request)
        {
            // STUB
            // Записываем в Redis что курьер связан с заказом
            // Может быть отправить через kafka сообщение клиенту о назначении курьера

            return Ok();
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.UpdateOwnDeliveryStatus | UserPermissions.UpdateOthersDeliveryStatus)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request)
        {
            UserAuthToken token = await authTokenStorage.GetToken(request.CourierTokenId);

            // STUB
            // Проверяем, что изменяет статус тот курьер, который назначен на заказ

            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderStatus, request.OrderId.ToString(), request);

            return Ok();
        }
    }
}
