using Confluent.Kafka;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.DeliveryService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer) : base(kafkaConsumer) { }

        protected override string[] Topics => new string[] { "order-created-events" };

        protected override Task HandleMessage(string message)
        {
            OrderCreatedEvent evt = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
            Console.WriteLine($"Получено сообщение о создании заказа: {evt.OrderId}");
            // STUB
            return null;
        }
    }
}
