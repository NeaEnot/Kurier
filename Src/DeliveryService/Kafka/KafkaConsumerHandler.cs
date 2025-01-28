using Confluent.Kafka;
using Kurier.Common;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Kafka;
using Kurier.Common.Models;
using Kurier.Common.Models.Events;
using System.Text.Json;

namespace Kurier.DeliveryService.Kafka
{
    public class KafkaConsumerHandler : AbstactKafkaConsumerHandler
    {
        private readonly IDeliveryStorage deliveryStorage;

        public KafkaConsumerHandler(IConsumer<string, string> kafkaConsumer, IDeliveryStorage deliveryStorage) : base(kafkaConsumer)
        {
            this.deliveryStorage = deliveryStorage;
        }

        protected override string[] Topics => new string[] { Constants.Topics.OrderCreatedEvents };

        protected override async Task HandleMessage(string message, string topic)
        {
            switch (topic)
            {
                case Constants.Topics.OrderCreatedEvents:
                    {
                        OrderCreatedEvent evt = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                        OrderDelivery delivery = new OrderDelivery
                        {
                            OrderId = evt.OrderId,
                            CourierId = null,
                            Status = OrderStatus.Created
                        };

                        deliveryStorage.CreateDelivery(delivery);
                        break;
                    }
                case Constants.Topics.OrderCanceledEvents:
                    {
                        Guid orderId = JsonSerializer.Deserialize<Guid>(message);
                        OrderDelivery delivery = await deliveryStorage.GetDeliveryById(orderId);

                        delivery.Status = OrderStatus.Canceled;
                        delivery.CourierId = null;

                        deliveryStorage.UpdateDelivery(delivery);
                        break;
                    }
            }
        }
    }
}
