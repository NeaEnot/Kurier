using Kurier.Common.Enums;
using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface IAuthTokenStorage
    {
        Task<UserAuthToken> CreateToken(Guid userId, UserPermissions permissions);
        Task<UserAuthToken> GetToken(Guid tokenId);
    }
}
