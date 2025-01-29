using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Kurier.RedisStorage
{
    public class RedisNotificationsStorage : INotificationsStorage
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisNotificationsStorage(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task SaveNotificationsList(NotificationsList notificationsList)
        {
            string key = GetNotificationsKey(notificationsList.UserId);
            string value = JsonSerializer.Serialize(notificationsList);

            await _db.StringSetAsync(key, value);
        }

        public async Task<NotificationsList> GetNotificationsList(Guid userId)
        {
            string key = GetNotificationsKey(userId);
            RedisValue value = await _db.StringGetAsync(key);

            if (value.IsNullOrEmpty)
            {
                return null;
            }

            NotificationsList notificationsList = JsonSerializer.Deserialize<NotificationsList>(value);
            return notificationsList;
        }

        public async Task DeleteNotificationsList(Guid userId)
        {
            string key = GetNotificationsKey(userId);
            _db.KeyDeleteAsync(key);
        }

        private string GetNotificationsKey(Guid userId) => $"user:{userId}";
    }
}
