using Kurier.Common;
using Kurier.Common.Attributes;
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
        private readonly IDeliveryStorage deliveryStorage;

        public DeliveryController(KafkaProducerHandler kafkaProducer, IDeliveryStorage deliveryStorage)
        {
            this.kafkaProducer = kafkaProducer;
            this.deliveryStorage = deliveryStorage;
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.AssignSelfToDelivery | UserPermissions.AssignOthersToDelivery)]
        public async Task<IActionResult> AssignCourierForDelivery([FromBody] AssignDeliveryRequest request)
        {
            OrderDelivery delivery = await deliveryStorage.GetDeliveryById(request.OrderId);
            delivery.CourierId = request.CourierId;
            deliveryStorage.UpdateDelivery(delivery);

            return Ok();
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.UpdateOwnDeliveryStatus | UserPermissions.UpdateOthersDeliveryStatus)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request)
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj as UserAuthToken;

            OrderDelivery delivery = await deliveryStorage.GetDeliveryById(request.OrderId);
            if (delivery.CourierId != token.UserId)
            {
                return Forbid();
            }

            delivery.Status = request.Status;

            deliveryStorage.UpdateDelivery(delivery);
            kafkaProducer.PublishEventAsync(Constants.Topics.OrderStatus, request.OrderId.ToString(), request);

            return Ok();
        }
    }
}
