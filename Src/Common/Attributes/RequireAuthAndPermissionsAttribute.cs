using Kurier.Common.Enums;
using Kurier.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kurier.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequireAuthAndPermissionsAttribute : Attribute, IAuthorizationFilter
    {
        private readonly UserPermissions requiredPermissions;

        public RequireAuthAndPermissionsAttribute(UserPermissions requiredPermissions)
        {
            this.requiredPermissions = requiredPermissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("UserToken", out var userAuthTokenObj))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            UserAuthToken userAuthToken = UserAuthToken.Parse(userAuthTokenObj.ToString());

            if (requiredPermissions > UserPermissions.None && !userAuthToken.Permissions.ContainsAny(requiredPermissions))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
