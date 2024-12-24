using Confluent.Kafka;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.ClientService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer) : base(kafkaConsumer) { }

        protected override string[] Topics => new string[] { "order-updated-events" };

        protected override Task HandleMessage(string message)
        {
            OrderUpdatedEvent evt = JsonSerializer.Deserialize<OrderUpdatedEvent>(message);

            // STUB
            // Получаем из Redis NotificationsList по evt.ClientId
            // если null, то:
            NotificationsList notificationsList = new NotificationsList { ClientId = evt.ClientId };
            notificationsList.Notifications.Add($"Заказ {evt.OrderId} переведён в статус {evt.NewStatus}");
            // сохраняем в Redis

            return null;
        }
    }
}
