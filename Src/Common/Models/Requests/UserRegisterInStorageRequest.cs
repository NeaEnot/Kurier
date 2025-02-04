using Kurier.Common.Enums;

namespace Kurier.Common.Models.Requests
{
    public class UserRegisterInStorageRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public UserPermissions Permissions { get; set; }
    }
}
