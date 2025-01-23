namespace Kurier.Common.Enums
{
    [Flags]
    public enum UserPermissions
    {
        None = 0,
        CreateOwnOrder = 1,
        CancelOwnOrder = 2,
        GetOwnOrder = 4,
        AssignSelfToDelivery = 8,
        UpdateOwnDeliveryStatus = 16,
        CreateOthersOrder = 32,
        CancelOthersOrder = 64,
        GetOthersOrder = 128,
        AssignOthersToDelivery = 256,
        UpdateOthersDeliveryStatus = 512,
        All = ~0
    }
}
