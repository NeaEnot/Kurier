using Confluent.Kafka;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.ClientService.Kafka
{
    public class KafkaConsumerHandler : BackgroundService
    {
        private readonly IConsumer<string, string> kafkaConsumer;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer)
        {
            this.kafkaConsumer = kafkaConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            kafkaConsumer.Subscribe("order-updated-events");

            await Task.Run(() =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = kafkaConsumer.Consume(stoppingToken);
                        if (consumeResult != null)
                            HandleMessage(consumeResult.Value);
                    }
                    catch (Exception ex)
                    { }
                }
            });
        }

        private async Task HandleMessage(string message)
        {
            OrderUpdatedEvent evt = JsonSerializer.Deserialize<OrderUpdatedEvent>(message);

            // STUB
            // Получаем из Redis NotificationsList по evt.ClientId
            // если null, то:
            NotificationsList notificationsList = new NotificationsList { ClientId = evt.ClientId };
            notificationsList.Notifications.Add($"Заказ {evt.OrderId} переведён в статус {evt.NewStatus}");
            // сохраняем в Redis
        }
    }
}
