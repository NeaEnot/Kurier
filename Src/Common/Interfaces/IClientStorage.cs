using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IClientStorage
    {
        Task Register(UserRequest request);
        Task<Guid> Auth(UserRequest request);
    }
}
