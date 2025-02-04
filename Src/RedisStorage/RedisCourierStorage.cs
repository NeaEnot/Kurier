using Kurier.Common.Interfaces;
using StackExchange.Redis;

namespace Kurier.RedisStorage
{
    public class RedisCourierStorage : ICourierStorage
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase db;

        public RedisCourierStorage(IConnectionMultiplexer redis)
        {
            this.redis = redis;
            db = this.redis.GetDatabase();
        }

        public async Task AddCourier(Guid courierId)
        {
            string key = GetKey();
            string value = courierId.ToString();

            db.ListRightPushAsync(key, value);
        }

        public async Task<List<Guid>> GetCouriers()
        {
            string key = GetKey();
            RedisValue[] values = await db.ListRangeAsync(key);

            List<Guid> couriers =
                values
                .Where(value => !value.IsNullOrEmpty)
                .Select(value => Guid.Parse(value))
                .ToList();

            return couriers;
        }

        public async Task DeleteCourier(Guid courierId)
        {
            string key = GetKey();
            string value = courierId.ToString();

            db.ListRemoveAsync(key, value);
        }

        private string GetKey() => $"couriers";
    }
}
