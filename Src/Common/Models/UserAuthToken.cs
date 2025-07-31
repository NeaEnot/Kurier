using Kurier.Common.Enums;

namespace Kurier.Common.Models
{
    public class UserAuthToken
    {
        public Guid TokenId { get; set; }
        public Guid UserId { get; set; }
        public UserPermissions Permissions { get; set; }
        public DateTime EndTime { get; set; }

        public override string ToString()
        {
            return $@"{TokenId};{UserId};{Permissions};{EndTime}";
        }

        public static UserAuthToken Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            string[] values = value.Split(';');

            return new UserAuthToken
            {
                TokenId = Guid.Parse(values[0]),
                UserId = Guid.Parse(values[1]),
                Permissions = (UserPermissions)Enum.Parse(typeof(UserPermissions), values[2]),
                EndTime = DateTime.Parse(values[3]),
            };
        }
    }
}
