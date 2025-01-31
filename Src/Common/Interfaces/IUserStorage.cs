using Kurier.Common.Models.Requests;

namespace Kurier.Common.Interfaces
{
    public interface IUserStorage
    {
        Task Register(UserRegisterRequest request);
        Task<Guid> Auth(UserAuthRequest request);
    }
}
