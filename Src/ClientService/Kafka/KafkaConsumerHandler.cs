using Confluent.Kafka;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.ClientService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer) : base(kafkaConsumer) { }

        protected override string[] Topics => new string[] { "order-updated-events", "order-canceled-events" };

        protected override Task HandleMessage(string message, string topic)
        {
            switch (topic)
            {
                case "order-updated-events":
                    OrderUpdatedEvent evt = JsonSerializer.Deserialize<OrderUpdatedEvent>(message);

                    // STUB
                    // Получаем из Redis NotificationsList по evt.ClientId
                    // если null, то:
                    NotificationsList notificationsList = new NotificationsList { ClientId = evt.ClientId };
                    notificationsList.Notifications.Add($"Заказ {evt.OrderId} переведён в статус {evt.NewStatus}");
                    // сохраняем в Redis
                    break;
                case "order-canceled-events":
                    Guid orderId = JsonSerializer.Deserialize<Guid>(message);
                    Console.WriteLine($"Получено сообщение об отмене заказа: {orderId}");
                    // STUB
                    break;
            }

            return null;
        }
    }
}
