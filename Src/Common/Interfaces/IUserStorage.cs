using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IUserStorage
    {
        Task Register(UserRequest request);
        Task<Guid> Auth(UserRequest request);
    }
}
