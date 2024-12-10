using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IOrderStorage
    {
        Task<Guid> CreateOrder(CreateOrderRequest request);
        Task<GetOrderResponse> GetOrderById (Guid id);
        Task UpdateOrderStatus(UpdateOrderStatusRequest request);
    }
}
