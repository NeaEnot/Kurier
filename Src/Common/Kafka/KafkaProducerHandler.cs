using Confluent.Kafka;
using System.Text.Json;

namespace Kurier.Common.Kafka
{
    public class KafkaProducerHandler
    {
        private readonly IProducer<string, string> kafkaProducer;

        public KafkaProducerHandler(IProducer<string, string> kafkaProducer)
        {
            this.kafkaProducer = kafkaProducer;
        }

        public async Task PublishEventAsync(string topic, string key, object message)
        {
            var serializedMessage = JsonSerializer.Serialize(message);
            await kafkaProducer.ProduceAsync(topic, new Message<string, string>
            {
                Key = key,
                Value = serializedMessage
            });
        }
    }
}
