using Confluent.Kafka;
using Kurier.Common;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models.Events;
using Kurier.Common.Models.Requests;
using System.Text.Json;

namespace Kurier.OrderService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        private IServiceProvider serviceProvider;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, IServiceProvider serviceProvider) : base(kafkaConsumer)
        {
            this.serviceProvider = serviceProvider;
        }

        protected override string[] Topics => new string[] { Constants.Topics.OrderStatus };

        protected override async Task HandleMessage(string message, string topic)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                var orderStorage = scope.ServiceProvider.GetRequiredService<IOrderStorage>();
                var kafkaProducer = scope.ServiceProvider.GetRequiredService<KafkaProducerHandler>();

                UpdateOrderStatusRequest request = JsonSerializer.Deserialize<UpdateOrderStatusRequest>(message);
                OrderUpdatedEvent evt = await orderStorage.UpdateOrderStatus(request);
                await kafkaProducer.PublishEventAsync(Constants.Topics.OrderUpdatedEvents, evt.OrderId.ToString(), evt);
            }
        }
    }
}
