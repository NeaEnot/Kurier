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
        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer) : base(kafkaConsumer) { }

        protected override string[] Topics => new string[] { Constants.Topics.OrderUpdatedEvents, Constants.Topics.OrderCanceledEvents };

        protected override Task HandleMessage(string message, string topic)
        {
            switch (topic)
            {
                case Constants.Topics.OrderUpdatedEvents:
                    OrderUpdatedEvent evt = JsonSerializer.Deserialize<OrderUpdatedEvent>(message);

                    // STUB
                    // Получаем из Redis NotificationsList по evt.ClientId
                    // если null, то:
                    NotificationsList notificationsList = new NotificationsList { ClientId = evt.ClientId };
                    notificationsList.Add($"Заказ {evt.OrderId} переведён в статус {evt.NewStatus}");
                    // сохраняем в Redis
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
