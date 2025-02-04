namespace Kurier.Common.Enums
{
    [Flags]
    public enum UserPermissions
    {
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
        CreateCouriers = 1024,
        CreateManagers = 2048,

        None = 0,
        All = ~0,

        Client = CreateOwnOrder | CancelOwnOrder | GetOwnOrder,
        Courier = AssignSelfToDelivery | UpdateOwnDeliveryStatus,
        Manager = CreateOthersOrder | CancelOthersOrder | GetOthersOrder | AssignOthersToDelivery | UpdateOthersDeliveryStatus | CreateCouriers | CreateManagers,
    }
}
