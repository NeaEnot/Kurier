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

        [HttpGet]
        [RequireAuthAndPermissions(UserPermissions.AssignSelfToDelivery | UserPermissions.AssignOthersToDelivery)]
        public async Task<IActionResult> GetDeliveriesForCourier(Guid? courierId)
        {
            if (courierId != null && !CanActionWithDelivery(courierId.Value, UserPermissions.AssignOthersToDelivery))
            {
                return Forbid();
            }

            List<OrderDelivery> deliveries = await deliveryStorage.GetDeliveriesForCourier(courierId);
            return Ok(deliveries);
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.AssignSelfToDelivery | UserPermissions.AssignOthersToDelivery)]
        public async Task<IActionResult> AssignCourierForDelivery([FromBody] AssignDeliveryRequest request)
        {
            if (!CanActionWithDelivery(request.CourierId, UserPermissions.AssignOthersToDelivery))
            {
                return Forbid();
            }

            OrderDelivery delivery = await deliveryStorage.GetDeliveryById(request.OrderId);
            delivery.CourierId = request.CourierId;
            deliveryStorage.UpdateDelivery(delivery);

            return Ok();
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.UpdateOwnDeliveryStatus | UserPermissions.UpdateOthersDeliveryStatus)]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request)
        {
            OrderDelivery delivery = await deliveryStorage.GetDeliveryById(request.OrderId);
            if (!CanActionWithDelivery(delivery.CourierId.Value, UserPermissions.UpdateOthersDeliveryStatus))
            {
                return Forbid();
            }

            delivery.Status = request.Status;

            deliveryStorage.UpdateDelivery(delivery);
            kafkaProducer.PublishEventAsync(Constants.Topics.OrderStatus, request.OrderId.ToString(), request);

            return Ok();
        }

        private bool CanActionWithDelivery(Guid courierId, UserPermissions permissions)
        {
            UserAuthToken token = GetUserToken();

            return courierId == token.UserId || token.Permissions.Contains(permissions);
        }

        private UserAuthToken GetUserToken()
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj as UserAuthToken;
            return token;
        }
    }
}
