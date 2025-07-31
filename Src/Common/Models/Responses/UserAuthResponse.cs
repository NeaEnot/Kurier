using Kurier.Common.Enums;

namespace Kurier.Common.Models.Responses
{
    public class UserAuthResponse
    {
        public Guid UserId {  get; set; }
        public UserPermissions Permissions { get; set; }
    }
}
