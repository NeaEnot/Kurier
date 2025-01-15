using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.ClientService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientsController : ControllerBase
    {
        private IUserStorage userStorage;
        private IAuthTokenStorage authTokenStorage;

        public ClientsController(IUserStorage clientStorage, IAuthTokenStorage authTokenStorage)
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
        public async Task<AuthResponse> Auth([FromBody] UserRequest request)
        {
            AuthResponse response;

            try
            {
                Guid clientId = await userStorage.Auth(request);

                UserAuthToken token = await authTokenStorage.CreateToken(clientId);

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

        [HttpGet]
        public async Task<NotificationsList> GetNotifications([FromBody] Guid tokenId)
        {
            UserAuthToken token = await authTokenStorage.GetToken(tokenId);

            // STUB
            // Получаем список уведомлений из Redis
            // Удаляем список уведомлений из Redis

            return null;
        }
    }
}
