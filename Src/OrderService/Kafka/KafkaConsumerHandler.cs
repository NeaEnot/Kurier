using Confluent.Kafka;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using System.Text.Json;

namespace Kurier.OrderService.Kafka
{
    public class KafkaConsumerHandler : BackgroundService
    {
        private readonly IConsumer<string, string> kafkaConsumer;
        private readonly IOrderStorage orderStorage;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, IOrderStorage orderStorage)
        {
            this.kafkaConsumer = kafkaConsumer;
            this.orderStorage = orderStorage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            kafkaConsumer.Subscribe("order-status");

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
        }

        private async Task HandleMessage(string message)
        {
            UpdateOrderStatusRequest request = JsonSerializer.Deserialize<UpdateOrderStatusRequest>(message);
            await orderStorage.UpdateOrderStatus(request);
        }
    }
}
