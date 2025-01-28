using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IDeliveryStorage
    {
        Task CreateDelivery(OrderDelivery request);
        Task<List<OrderDelivery>> GetDeliveriesForCourier(Guid? courierId);
        Task<OrderDelivery> GetDeliveryById(Guid orderId);
        Task UpdateDelivery(OrderDelivery request);
    }
}
