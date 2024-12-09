using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.Common
{
    public class KafkaService
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IConsumer<Null, string> _consumer;

        private readonly IDictionary<string, string> _producerConfig;
        private readonly IDictionary<string, string> _consumerConfig;

        public KafkaService() : this("localhost") { }

        public KafkaService(string host)
        {
            _producerConfig = new Dictionary<string, string> { { "bootstrap.servers", host } };
            _consumerConfig = new Dictionary<string, string>
            {
                { "group.id", "custom-group"},
                { "bootstrap.servers", host }
            };

            _producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            _consumer = new ConsumerBuilder<Null, string>(_consumerConfig).Build();
        }

        public async Task SendMessageAsync(string topic, string message)
        {
            try
            {
                var kafkaMessage = new Message<Null, string> {  Value = message };
                var result = await _producer.ProduceAsync(topic, kafkaMessage);
            }
            catch (Exception ex)
            { }
        }

        public void SubscribeOnTopic(string topic, Action<string> action, CancellationToken cancellationToken)
        {
            _consumer.Subscribe(topic);

            // Если оставить Task.Run, то работает сервис, но апи зависает без ответа
            Task.Run(() =>
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var result = _consumer.Consume(cancellationToken);
                            var message = result.Message.Value;

                            action.Invoke(message);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                        catch (ConsumeException ex)
                        {
                            Console.WriteLine($"Error while consuming message: {ex.Message}");
                        }
                    }
                }
                finally
                {
                    _consumer.Close(); // Закрываем consumer при завершении
                }
            }, cancellationToken);
        }

        public void Dispose()
        {
            _producer?.Dispose();
            _consumer?.Dispose();
        }
    }
}
