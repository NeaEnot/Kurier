using Kurier.Common.Enums;

namespace Kurier.Common.Models.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserPermissions Permissions { get; set; }

        public List<Order> Orders { get; protected set; }
        public User() { }
    }
}
