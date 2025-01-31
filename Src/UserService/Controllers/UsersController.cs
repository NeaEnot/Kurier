using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Kurier.Common.Attributes;
using UserService.Attributes;
using Kurier.Common.Models.Requests;

using UP = Kurier.Common.Enums.UserPermissions;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserStorage userStorage;
        private readonly INotificationsStorage notificationsStorage;
        private readonly IAuthTokenStorage authTokenStorage;

        private static UP courierPermissions = UP.AssignSelfToDelivery | UP.UpdateOwnDeliveryStatus;
        private static UP managerPermissions =
            UP.CreateOthersOrder | UP.CancelOthersOrder | UP.GetOthersOrder |
            UP.AssignOthersToDelivery | UP.UpdateOthersDeliveryStatus |
            UP.CreateCouriers | UP.CreateManagers;

        public UsersController(IUserStorage userStorage, INotificationsStorage notificationsStorage, IAuthTokenStorage authTokenStorage)
        {
            this.userStorage = userStorage;
            this.notificationsStorage = notificationsStorage;
            this.authTokenStorage = authTokenStorage;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj != null ? userAuthTokenObj as UserAuthToken : null;

            if (request.Permissions.ContainsAny(courierPermissions) && (token == null || !token.Permissions.ContainsAll(UP.CreateCouriers)))
            {
                return Forbid();
            }

            if (request.Permissions.ContainsAny(managerPermissions) && (token == null || !token.Permissions.ContainsAll(UP.CreateManagers)))
            {
                return Forbid();
            }

            await userStorage.Register(request);
            return Ok();
        }

        [HttpPost]
        public async Task<UserAuthToken> Auth([FromBody] UserAuthRequest request)
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
