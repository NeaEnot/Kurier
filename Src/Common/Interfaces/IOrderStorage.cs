using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IOrderStorage
    {
        Task<Guid> CreateOrder(CreateOrderInStorageRequest request);
        Task<GetOrderResponse> GetOrderById (Guid id);
        Task<OrderUpdatedEvent> UpdateOrderStatus(UpdateOrderStatusRequest request);
    }
}
