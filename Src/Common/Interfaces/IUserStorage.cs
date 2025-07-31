using Kurier.Common.Models.Requests;
using Kurier.Common.Models.Responses;

namespace Kurier.Common.Interfaces
{
    public interface IUserStorage
    {
        Task Register(UserRegisterInStorageRequest request);
        Task<UserAuthResponse> Auth(UserAuthRequest request);
    }
}
