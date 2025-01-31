namespace Kurier.Common.Enums
{
    public static class UserPermissionsMethods
    {
        public static bool Contains(this UserPermissions permissions, UserPermissions required)
        {
            return (permissions & required) == required;
        }
    }
}
