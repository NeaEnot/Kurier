using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.DeliveryService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CourierController : ControllerBase
    {
        private IUserStorage userStorage;
        private IAuthTokenStorage authTokenStorage;

        public CourierController(IUserStorage clientStorage, IAuthTokenStorage authTokenStorage)
        {
            this.userStorage = clientStorage;
            this.authTokenStorage = authTokenStorage;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            try
            {
                await userStorage.Register(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<UserAuthToken> Auth([FromBody] UserRequest request)
        {
            Guid courierId = await userStorage.Auth(request);
            UserAuthToken token = await authTokenStorage.CreateToken(courierId);

            return token;
        }
    }
}
