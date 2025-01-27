using Confluent.Kafka;
using Kurier.Common;
using Kurier.Common.Kafka;
using Kurier.Common.Models.Events;
using System.Text.Json;

namespace Kurier.DeliveryService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer) : base(kafkaConsumer) { }

        protected override string[] Topics => new string[] { Constants.Topics.OrderCreatedEvents };

        protected override Task HandleMessage(string message, string topic)
        {
            switch (topic)
            {
                case Constants.Topics.OrderCreatedEvents:
                    OrderCreatedEvent evt = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
                    Console.WriteLine($"Получено сообщение о создании заказа: {evt.OrderId}");
                    // STUB
                    break;
                    case Constants.Topics.OrderCanceledEvents:
                    Guid orderId = JsonSerializer.Deserialize<Guid>(message);
                    Console.WriteLine($"Получено сообщение об отмене заказа: {orderId}");
                    // STUB
                    break;
            }

            return null;
        }
    }
}
