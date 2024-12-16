using Confluent.Kafka;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.DeliveryService.Kafka
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
            kafkaConsumer.Subscribe("order-events");

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
            OrderCreatedEvent evt = JsonSerializer.Deserialize<OrderCreatedEvent>(message);
            Console.WriteLine($"Получено сообщение о создании заказа: {evt.OrderId}");
            // STUB
        }
    }
}
