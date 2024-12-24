using Confluent.Kafka;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.OrderService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        private readonly IOrderStorage orderStorage;
        private readonly KafkaProducerHandler kafkaProducer;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, IOrderStorage orderStorage, KafkaProducerHandler kafkaProducer) : base(kafkaConsumer)
        {
            this.orderStorage = orderStorage;
            this.kafkaProducer = kafkaProducer;
        }

        protected override string[] Topics => new string[] { "order-status" };

        protected override async Task HandleMessage(string message)
        {
            UpdateOrderStatusRequest request = JsonSerializer.Deserialize<UpdateOrderStatusRequest>(message);
            OrderUpdatedEvent evt = await orderStorage.UpdateOrderStatus(request);
            await kafkaProducer.PublishEventAsync("order-updated-events", evt.OrderId.ToString(), evt);
        }
    }
}
