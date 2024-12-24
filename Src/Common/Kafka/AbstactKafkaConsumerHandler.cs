using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Kurier.Common.Kafka
{
    public abstract class AbstactKafkaConsumerHandler : BackgroundService
    {
        private readonly IConsumer<string, string> kafkaConsumer;

        public AbstactKafkaConsumerHandler(IConsumer<string, string> kafkaConsumer)
        {
            this.kafkaConsumer = kafkaConsumer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            kafkaConsumer.Subscribe(Topics);

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

        protected abstract string[] Topics { get; }

        protected abstract Task HandleMessage(string message);
    }
}
