using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private IUserStorage userStorage;
        private IAuthTokenStorage authTokenStorage;

        public UsersController(IUserStorage userStorage, IAuthTokenStorage authTokenStorage)
        {
            this.userStorage = userStorage;
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
            Guid clientId = await userStorage.Auth(request);
            UserAuthToken token = await authTokenStorage.CreateToken(clientId);

            return token;
        }

        [HttpGet]
        public async Task<UserAuthToken> GetUserInfo(Guid tokenId)
        {
            UserAuthToken token = await authTokenStorage.GetToken(tokenId);
            return token;
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
