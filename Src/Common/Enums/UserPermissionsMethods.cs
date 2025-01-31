namespace Kurier.Common.Enums
{
    public static class UserPermissionsMethods
    {
        public static bool ContainsAny(this UserPermissions permissions, UserPermissions required)
        {
            return (permissions | required) > 0;
        }

        public static bool ContainsAll(this UserPermissions permissions, UserPermissions required)
        {
            return (permissions & required) == required;
        }
    }
}
