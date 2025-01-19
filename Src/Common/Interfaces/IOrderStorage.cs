using Kurier.Common.Models;
using Kurier.Common.Models.Events;

namespace Kurier.Common.Interfaces
{
    public interface IOrderStorage
    {
        Task<Guid> CreateOrder(CreateOrderInStorageRequest request);
        Task<GetOrderResponse> GetOrderById (Guid id);
        Task<OrderUpdatedEvent> UpdateOrderStatus(UpdateOrderStatusRequest request);
    }
}
