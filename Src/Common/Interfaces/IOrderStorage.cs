using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IOrderStorage
    {
        Task<Guid> CreateOrder(CreateOrderRequest request);
        Task<GetOrderResponce> GetOrderById (Guid id);
    }
}
