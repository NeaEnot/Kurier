using Kurier.Common;

namespace Kurier.RateService
{
    internal class Program
    {
        private static KafkaService kafkaService;
        private static string topic;

        static void Main(string[] args)
        {
            topic = "example-topic";
            kafkaService = new KafkaService();

            CancellationToken cancellationToken = new CancellationTokenSource().Token;
            kafkaService.SubscribeOnTopic<decimal>(topic, weight =>
            {
                RateOrder(weight);
            }, cancellationToken);
        }

        static async void RateOrder(decimal weight)
        {
            decimal price = Math.Round(weight * 3.125m, 2);
            kafkaService.SendMessageAsync(topic, price.ToString());
        }
    }
}