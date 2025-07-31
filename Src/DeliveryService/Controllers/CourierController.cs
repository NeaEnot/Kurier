using Kurier.Common.Attributes;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourierController(ICourierStorage courierStorage, ILogger<CourierController> logger) : ControllerBase
    {
        private readonly ICourierStorage _courierStorage = courierStorage;
        private readonly ILogger<CourierController> _logger = logger;

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.Courier)]
        public async Task<IActionResult> StartWork()
        {

            _logger.LogDebug("Work has been started.");

            UserAuthToken token = GetUserToken();

            _courierStorage.AddCourier(token.UserId);

            return Ok();
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.Courier)]
        public async Task<IActionResult> EndWork()
        {

            _logger.LogDebug("Work has been ended.");

            UserAuthToken token = GetUserToken();

            _courierStorage.DeleteCourier(token.UserId);

            return Ok();
        }

        [HttpGet]
        [RequireAuthAndPermissions(UserPermissions.AssignOthersToDelivery | UserPermissions.UpdateOthersDeliveryStatus)]
        public async Task<IActionResult> GetWorkedCouriers()
        {

            _logger.LogInformation("GetWorkedCouriers request has been received.");

            List<Guid> couriers = await _courierStorage.GetCouriers();

            return Ok(couriers);
        }

        private UserAuthToken GetUserToken()
        {
            HttpContext.Request.Headers.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = UserAuthToken.Parse(userAuthTokenObj.ToString());
            return token;
        }
    }
}
