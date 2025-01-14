using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kurier.ClientService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ClientsController : ControllerBase
    {
        private IClientStorage clientStorage;

        public ClientsController(IClientStorage clientStorage)
        {
            this.clientStorage = clientStorage;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            try
            {
                await clientStorage.Register(request);

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
                Guid clientId = await clientStorage.Auth(request);

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
                    TokenId = token.TokenId,
                    Message = "Ok"
                };
            }
            catch
            (Exception ex)
            {
                response = new AuthResponse
                {
                    TokenId = null,
                    Message = ex.Message
                };
            }

            return response;
        }

        [HttpGet]
        public async Task<NotificationsList> GetNotifications([FromBody] Guid clientId)
        {
            // STUB
            // Получаем список уведомлений из Redis
            // Удаляем список уведомлений из Redis

            return null;
        }
    }
}
