using Kurier.Common.Enums;

namespace Kurier.Common.Models.Requests
{
    public class UserRegisterInStorageRequest
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public UserPermissions Permissions { get; set; }
    }
}
