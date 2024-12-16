namespace Kurier.Common.Models
{
    public class NotificationsList
    {
        public Guid ClientId { get; set; }
        public List<string> Notifications { get; set; }

        public NotificationsList()
        {
            Notifications = new List<string>();
        }
    }
}
