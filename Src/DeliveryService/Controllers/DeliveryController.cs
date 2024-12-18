﻿using Kurier.Common.Models;
using Kurier.DeliveryService.Kafka;
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
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request)
        {
            await kafkaProducer.PublishEventAsync("order-status", request.OrderId.ToString(), request);

            return Ok();
        }
    }
}
