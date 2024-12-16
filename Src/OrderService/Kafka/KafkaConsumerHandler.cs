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
        private readonly KafkaProducerHandler kafkaProducer;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, IOrderStorage orderStorage, KafkaProducerHandler kafkaProducer)
        {
            this.kafkaConsumer = kafkaConsumer;
            this.orderStorage = orderStorage;
            this.kafkaProducer = kafkaProducer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            kafkaConsumer.Subscribe("order-status");

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
            UpdateOrderStatusRequest request = JsonSerializer.Deserialize<UpdateOrderStatusRequest>(message);
            OrderUpdatedEvent evt = await orderStorage.UpdateOrderStatus(request);
            await kafkaProducer.PublishEventAsync("order-updated-events", evt.OrderId.ToString(), evt);
        }
    }
}
