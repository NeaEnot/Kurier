using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Kurier.RedisStorage
{
    public class RedisAuthTokenStorage : IAuthTokenStorage
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisAuthTokenStorage(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task<UserAuthToken> CreateToken(Guid userId)
        {
            UserAuthToken token = new UserAuthToken
            {
                TokenId = Guid.NewGuid(),
                UserId = userId,
                EndTime = DateTime.Now.AddMinutes(20)
            };

            string key = GetTokenKey(token.TokenId);
            string value = JsonSerializer.Serialize(token);

            await _db.StringSetAsync(key, value);
            DeleteTokenByTime(key, token.EndTime);

            return token;
        }

        public async Task<UserAuthToken> GetToken(Guid tokenId)
        {
            string key = GetTokenKey(tokenId);
            RedisValue value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                throw new KeyNotFoundException($"Token with ID {tokenId} not found or expired.");
            }

            UserAuthToken token = JsonSerializer.Deserialize<UserAuthToken>(value);

            return token;
        }

        private async Task DeleteTokenByTime(string key, DateTime endTime)
        {
            Thread.Sleep(DateTime.Now - endTime);
            _db.KeyDeleteAsync(key);
        }

        private string GetTokenKey(Guid tokenId) => $"token:{tokenId}";
    }
}
