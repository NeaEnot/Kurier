using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IAuthTokenStorage
    {
        Task<UserAuthToken> CreateToken(Guid userId);
        Task<UserAuthToken> GetToken(Guid tokenId);
    }
}
