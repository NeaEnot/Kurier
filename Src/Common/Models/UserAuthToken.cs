using Kurier.Common.Enums;

namespace Kurier.Common.Models
{
    public class UserAuthToken
    {
        public Guid TokenId { get; set; }
        public Guid UserId { get; set; }
        public UserPermissions Permissions { get; set; }
        public DateTime EndTime { get; set; }
    }
}
