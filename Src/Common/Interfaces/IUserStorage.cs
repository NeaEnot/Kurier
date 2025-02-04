using Kurier.Common.Models.Requests;

namespace Kurier.Common.Interfaces
{
    public interface IUserStorage
    {
        Task Register(UserRegisterInStorageRequest request);
        Task<Guid> Auth(UserAuthRequest request);
    }
}
