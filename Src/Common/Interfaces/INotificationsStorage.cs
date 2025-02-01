using Kurier.Common.Models;

namespace Kurier.Common.Interfaces
{
    public interface INotificationsStorage
    {
        Task SaveNotificationsList(NotificationsList notificationsList);
        Task<NotificationsList> GetNotificationsList(Guid userId);
        Task DeleteNotificationsList(Guid userId);
    }
}
