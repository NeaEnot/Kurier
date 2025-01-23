using Kurier.Common;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Kurier.Common.Models.Events;
using Kurier.Common.Models.Requests;
using Kurier.Common.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace Kurier.OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderStorage orderStorage;
        private readonly KafkaProducerHandler kafkaProducer;
        private readonly HttpClient httpClient;

        public OrdersController(IOrderStorage orderStorage, KafkaProducerHandler kafkaProducer, IHttpClientFactory httpClientFactory)
        {
            this.orderStorage = orderStorage;
            this.kafkaProducer = kafkaProducer;
            httpClient = httpClientFactory.CreateClient("ApiGateway");
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.CreateOwnOrder | UserPermissions.CreateOthersOrder)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            UserAuthToken clientResponse = await GetClientInfo(request.ClientTokenId);
            CreateOrderInStorageRequest storageRequest = new CreateOrderInStorageRequest
            {
                ClientId = clientResponse.UserId,
                DepartureAddress = request.DepartureAddress,
                DeliveryAddress = request.DeliveryAddress,
                Weight = request.Weight
            };

            Guid id = await orderStorage.CreateOrder(storageRequest);

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
        [RequireAuthAndPermissions(UserPermissions.GetOwnOrder | UserPermissions.GetOthersOrder)]
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
        [RequireAuthAndPermissions(UserPermissions.CancelOwnOrder | UserPermissions.CancelOthersOrder)]
        public async Task<IActionResult> CancelOrder(CancelOrderRequest request)
        {
            UserAuthToken clientResponse = await GetClientInfo(request.ClientTokenId);
            GetOrderResponse order = await orderStorage.GetOrderById(request.OrderId);

            if (order == null || order.ClientId != clientResponse.UserId)
            {
                return NotFound();
            }

            UpdateOrderStatusRequest storageRequest = new UpdateOrderStatusRequest { OrderId = request.OrderId, Status = OrderStatus.Canceled };
            await orderStorage.UpdateOrderStatus(storageRequest);

            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderCanceledEvents, request.OrderId.ToString(), request.OrderId);

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
