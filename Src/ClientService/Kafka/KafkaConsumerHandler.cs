using Confluent.Kafka;
using Kurier.Common;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Kurier.Common.Models.Events;
using System.Text.Json;

namespace Kurier.ClientService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer) : base(kafkaConsumer) { }

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, INotificationsStorage notificationsStorage) : base(kafkaConsumer)
        {
            this.notificationsStorage = notificationsStorage;
        }

        protected override string[] Topics => new string[] { Constants.Topics.OrderUpdatedEvents };

        protected override Task HandleMessage(string message, string topic)
        {
            OrderUpdatedEvent evt = JsonSerializer.Deserialize<OrderUpdatedEvent>(message);

            NotificationsList notificationsList = await notificationsStorage.GetNotificationsList(evt.ClientId);

            if (notificationsList == null)
                notificationsList = new NotificationsList { UserId = evt.ClientId };

            notificationsList.Add($"Заказ {evt.OrderId} переведён в статус {evt.NewStatus}");

            notificationsStorage.SaveNotificationsList(notificationsList);
        }
    }
}
