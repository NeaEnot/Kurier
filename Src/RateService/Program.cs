using Kurier.Common;

namespace Kurier.RateService
{
    internal class Program
    {
        private static KafkaService kafkaService;
        private static string topicIn;
        private static string topicOut;

        static async Task Main(string[] args)
        {
            topicIn = "example-topic-in";
            topicOut = "example-topic-out";
            kafkaService = new KafkaService();

            CancellationTokenSource cts = new CancellationTokenSource();
            kafkaService.SubscribeOnTopic(topicIn, message => RateOrder(decimal.Parse(message)), cts.Token);

            Console.WriteLine("Press Enter to exit...");
            await Task.Run(() => Console.ReadLine());
            cts.Cancel();
        }

        static void RateOrder(decimal weight)
        {
            decimal price = Math.Round(weight * 3.125m, 2);
            Console.WriteLine($"Rated order with weight {weight} by price {price}");

            kafkaService.SendMessageAsync(topicOut, price.ToString());
        }
    }
}