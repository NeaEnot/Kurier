using Kurier.Common.Interfaces;
using Kurier.Common.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace Kurier.RedisStorage
{
    public class RedisNotificationsStorage : INotificationsStorage
    {
        private readonly IConnectionMultiplexer redis;
        private readonly IDatabase db;

        public RedisNotificationsStorage(IConnectionMultiplexer redis)
        {
            this.redis = redis;
            db = redis.GetDatabase();
        }

        public async Task SaveNotificationsList(NotificationsList notificationsList)
        {
            string key = GetNotificationsKey(notificationsList.UserId);
            string value = JsonSerializer.Serialize(notificationsList);

            await db.StringSetAsync(key, value);
        }

        public async Task<NotificationsList> GetNotificationsList(Guid userId)
        {
            string key = GetNotificationsKey(userId);
            RedisValue value = await db.StringGetAsync(key);

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
            db.KeyDeleteAsync(key);
        }

        private string GetNotificationsKey(Guid userId) => $"user:{userId}";
    }
}
