﻿using Kurier.Common.Enums;
using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Kurier.Common.Attributes;
using UserService.Attributes;
using Kurier.Common.Models.Requests;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController(IUserStorage userStorage, INotificationsStorage notificationsStorage,
        IAuthTokenStorage authTokenStorage, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserStorage _userStorage = userStorage;
        private readonly INotificationsStorage _notificationsStorage = notificationsStorage;
        private readonly IAuthTokenStorage _authTokenStorage = authTokenStorage;
        private readonly ILogger<UsersController> _logger = logger;

        private readonly Dictionary<UserRole, UserPermissions> permissions = new Dictionary<UserRole, UserPermissions>
        {
            { UserRole.Client, UserPermissions.Client },
            { UserRole.Courier, UserPermissions.Courier },
            { UserRole.Manager, UserPermissions.Manager }
        };

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj != null ? userAuthTokenObj as UserAuthToken : null;

            if (request.Role == UserRole.Courier && (token == null || !token.Permissions.ContainsAll(UserPermissions.CreateCouriers)))
            {
                return Forbid();
            }

            if (request.Role == UserRole.Manager && (token == null || !token.Permissions.ContainsAll(UserPermissions.CreateManagers)))
            {
                return Forbid();
            }

            UserRegisterInStorageRequest requestInStorage = new UserRegisterInStorageRequest
            {
                Email = request.Login,
                Password = request.Password,
                Permissions = permissions[request.Role]
            };

            await _userStorage.Register(requestInStorage);
            return Ok();
        }

        [HttpPost]
        public async Task<Guid> Auth([FromBody] UserAuthRequest request)
        {
            Guid clientId = await _userStorage.Auth(request);
            UserAuthToken token = await _authTokenStorage.CreateToken(clientId);

            return token.TokenId;
        }

        [HttpGet]
        [TrustedKeys]
        public async Task<UserAuthToken> GetUserInfo(Guid tokenId)
        {
            UserAuthToken token = await _authTokenStorage.GetToken(tokenId);
            return token;
        }

        [HttpGet]
        [RequireAuthAndPermissions(UserPermissions.None)]
        public async Task<NotificationsList> GetNotifications()
        {
            HttpContext.Items.TryGetValue("UserToken", out var userAuthTokenObj);
            UserAuthToken token = userAuthTokenObj as UserAuthToken;

            NotificationsList notifications = await _notificationsStorage.GetNotificationsList(token.UserId);
            await _notificationsStorage.DeleteNotificationsList(token.UserId);

            return notifications;
        }
    }
}
