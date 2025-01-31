using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Kurier.Common.Attributes;
using UserService.Attributes;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserStorage userStorage;
        private readonly INotificationsStorage notificationsStorage;
        private readonly IAuthTokenStorage authTokenStorage;

        public UsersController(IUserStorage userStorage, INotificationsStorage notificationsStorage, IAuthTokenStorage authTokenStorage)
        {
            this.userStorage = userStorage;
            this.notificationsStorage = notificationsStorage;
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
        [TrustedKeys]
        public async Task<UserAuthToken> GetUserInfo(Guid tokenId)
        {
            UserAuthToken token = await authTokenStorage.GetToken(tokenId);
            return token;
        }

        [HttpGet]
        [RequireAuthAndPermissions(UserPermissions.None)]
        public async Task<NotificationsList> GetNotifications()
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj as UserAuthToken;

            NotificationsList notifications = await notificationsStorage.GetNotificationsList(token.UserId);
            await notificationsStorage.DeleteNotificationsList(token.UserId);

            return notifications;
        }
    }
}
