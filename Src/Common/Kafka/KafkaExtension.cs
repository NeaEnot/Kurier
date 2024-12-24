using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Kurier.Common.Kafka
{
    public static class KafkaExtension
    {
        public static IServiceCollection AddKafka<TConsumerHandler>(this IServiceCollection services) where TConsumerHandler : AbstactKafkaConsumerHandler
        {
            return services
                .AddSingleton<IProducer<string, string>>(sp =>
                {
                    var config = new ProducerConfig
                    {
                        BootstrapServers = "localhost:9092",
                        EnableIdempotence = true,
                        Acks = Acks.All
                    };
                    return new ProducerBuilder<string, string>(config).Build();
                })
                .AddSingleton<IConsumer<string, string>>(sp =>
                {
                    var config = new ConsumerConfig
                    {
                        GroupId = "kurier-group",
                        BootstrapServers = "localhost:9092",
                        AutoOffsetReset = AutoOffsetReset.Earliest
                    };
                    return new ConsumerBuilder<string, string>(config).Build();
                })
                .AddSingleton<KafkaProducerHandler>()
                .AddHostedService<TConsumerHandler>();
        }
    }
}
