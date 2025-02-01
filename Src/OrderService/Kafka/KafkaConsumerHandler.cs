using Confluent.Kafka;
using Kurier.Common;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models.Events;
using Kurier.Common.Models.Requests;
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

        protected override string[] Topics => new string[] { Constants.Topics.OrderStatus };

        protected override async Task HandleMessage(string message, string topic)
        {
            UpdateOrderStatusRequest request = JsonSerializer.Deserialize<UpdateOrderStatusRequest>(message);
            OrderUpdatedEvent evt = await orderStorage.UpdateOrderStatus(request);
            await kafkaProducer.PublishEventAsync(Constants.Topics.OrderUpdatedEvents, evt.OrderId.ToString(), evt);
        }
    }
}
