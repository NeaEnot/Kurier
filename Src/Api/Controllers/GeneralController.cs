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
        public decimal RateOrder(decimal weight)
        {
            return 10;
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
