using Kurier.Common.Attributes;
using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourierController : ControllerBase
    {
        private readonly ICourierStorage courierStorage;

        public CourierController(ICourierStorage courierStorage)
        {
            this.courierStorage = courierStorage;
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.Courier)]
        public async Task<IActionResult> StartWork()
        {
            UserAuthToken token = GetUserToken();

            courierStorage.AddCourier(token.UserId);

            return Ok();
        }

        [HttpPost]
        [RequireAuthAndPermissions(UserPermissions.Courier)]
        public async Task<IActionResult> EndWork()
        {
            UserAuthToken token = GetUserToken();

            courierStorage.DeleteCourier(token.UserId);

            return Ok();
        }

        [HttpGet]
        [RequireAuthAndPermissions(UserPermissions.Manager)]
        public async Task<IActionResult> GetWorkedCouriers()
        {
            List<Guid> couriers = courierStorage.GetCouriers();

            return Ok(couriers);
        }

        private UserAuthToken GetUserToken()
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj as UserAuthToken;
            return token;
        }
    }
}
