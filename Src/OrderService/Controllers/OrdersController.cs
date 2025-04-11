using Kurier.Common;
using Kurier.Common.Attributes;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Kurier.Common.Models.Events;
using Kurier.Common.Models.Requests;
using Kurier.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kurier.OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrdersController(IOrderStorage orderStorage, KafkaProducerHandler kafkaProducer,
        IHttpClientFactory httpClientFactory, ILogger<OrdersController> logger) : ControllerBase
    {
        private readonly IOrderStorage _orderStorage = orderStorage;
        private readonly KafkaProducerHandler _kafkaProducer = kafkaProducer;
        private readonly HttpClient httpClient = httpClientFactory.CreateClient("ApiGateway");
        private readonly ILogger<OrdersController> _logger = logger;

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.CreateOwnOrder | UserPermissions.CreateOthersOrder)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {

            _logger.LogInformation($"CreateOrder request has been received. Request:{request}");

            UserAuthToken clientResponse = await GetClientInfo(request.ClientTokenId); // TODO: переделать логику, чтобы и менеджер мог создать заказ для пользователя
            CreateOrderInStorageRequest storageRequest = new CreateOrderInStorageRequest
            {
                ClientId = clientResponse.UserId,
                DepartureAddress = request.DepartureAddress,
                DeliveryAddress = request.DeliveryAddress,
                Weight = request.Weight
            };

            Guid id = await _orderStorage.CreateOrder(storageRequest);

            OrderCreatedEvent evt = new OrderCreatedEvent
            {
                OrderId = id,
                Weight = request.Weight,
                DepartureAddress = request.DepartureAddress,
                DeliveryAddress = request.DeliveryAddress
            };

            await _kafkaProducer.PublishEventAsync(Constants.Topics.OrderCreatedEvents, id.ToString(), evt);

            return Ok(id);
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.GetOwnOrder | UserPermissions.GetOthersOrder)]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {

            _logger.LogInformation($"GetOrderById request has been received. OrderId:{orderId}");

            var order = await _orderStorage.GetOrderById(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet]
        [RequireAuthAndPermissions(UserPermissions.CancelOwnOrder | UserPermissions.CancelOthersOrder)]
        public async Task<IActionResult> CancelOrder(CancelOrderRequest request)
        {

            _logger.LogInformation($"CancelOrder request has been received. Request:{request}");

            UserAuthToken clientResponse = await GetClientInfo(request.ClientTokenId);
            GetOrderResponse order = await _orderStorage.GetOrderById(request.OrderId);

            if (order == null || order.ClientId != clientResponse.UserId)
            {
                return NotFound();
            }

            UpdateOrderStatusRequest storageRequest = new UpdateOrderStatusRequest { OrderId = request.OrderId, Status = OrderStatus.Canceled };
            OrderUpdatedEvent evt = await _orderStorage.UpdateOrderStatus(storageRequest);

            _kafkaProducer.PublishEventAsync(Constants.Topics.OrderCanceledEvents, request.OrderId.ToString(), request.OrderId);
            _kafkaProducer.PublishEventAsync(Constants.Topics.OrderUpdatedEvents, evt.OrderId.ToString(), evt);

            return Ok();
        }

        private async Task<UserAuthToken> GetClientInfo(Guid clientTokenId)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, $"/api/users/GetUserInfo?tokenId={clientTokenId}");

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);
            string content = await httpResponse.Content.ReadAsStringAsync();
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new Exception(content);
            }

            return JsonConvert.DeserializeObject<UserAuthToken>(content);
        }
    }
}
