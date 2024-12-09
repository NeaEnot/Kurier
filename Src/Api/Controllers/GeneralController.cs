using Kurier.Common;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GeneralController : ControllerBase
    {
        private readonly ILogger<GeneralController> _logger;

        public GeneralController(ILogger<GeneralController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> RateOrder(decimal weight)
        {
            KafkaService kafkaService = new KafkaService();
            string topicIn = "example-topic-in";
            string topicOut = "example-topic-out";

            var tcs = new TaskCompletionSource<decimal>();
            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
            {
                kafkaService.SubscribeOnTopic(topicOut, message =>
                {
                    tcs.TrySetResult(decimal.Parse(message));
                }, cts.Token);

                await kafkaService.SendMessageAsync(topicIn, weight.ToString());

                try
                {
                    decimal answer = await tcs.Task;
                    return Ok(answer);
                }
                catch (TaskCanceledException)
                {
                    return StatusCode(504, "Timeout while waiting for Kafka response.");
                }
            }
        }

        [HttpPost]
        public string CreateOrder(OrderInput order)
        {
            return Guid.NewGuid().ToString();
        }

        [HttpGet]
        public OrderState GetOrderState(string orderId)
        {
            return OrderState.InProgress;
        }
    }
}
