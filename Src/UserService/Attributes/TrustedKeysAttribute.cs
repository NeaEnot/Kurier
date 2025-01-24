using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserService.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class TrustedKeysAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            IConfiguration configuration = context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;
            List<string> keys = configuration.GetSection("TrustedKeys").GetChildren().Select(x => x.Value).ToList();

            if (!context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var extractedApiKey) || !keys.Contains(extractedApiKey))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
