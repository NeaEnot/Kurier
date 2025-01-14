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

        public CourierController(IUserStorage clientStorage)
        {
            this.userStorage = clientStorage;
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
        public async Task<AuthResponse> Auth([FromBody] UserRequest request)
        {
            AuthResponse response;

            try
            {
                Guid clientId = await userStorage.Auth(request);

                UserAuthToken token = new UserAuthToken
                {
                    TokenId = Guid.NewGuid(),
                    UserId = clientId,
                    EndTime = DateTime.Now.AddMinutes(20)
                };

                // STUB
                // Записываем token в redis

                response = new AuthResponse
                {
                    Token = token,
                    Message = "Ok"
                };
            }
            catch
            (Exception ex)
            {
                response = new AuthResponse
                {
                    Token = null,
                    Message = ex.Message
                };
            }

            return response;
        }
    }
}
