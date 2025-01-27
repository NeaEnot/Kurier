using Confluent.Kafka;
using Kurier.Common.Kafka;
using Kurier.Common.Models.Events;
using Kurier.Common.Models;
using System.Text.Json;
using Kurier.Common;

namespace UserService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        private readonly INotificationsStorage notificationsStorage;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, INotificationsStorage notificationsStorage) : base(kafkaConsumer)
        {
            this.notificationsStorage = notificationsStorage;
        }

        protected override string[] Topics => new string[] { Constants.Topics.OrderUpdatedEvents };

        protected override async Task HandleMessage(string message, string topic)
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
