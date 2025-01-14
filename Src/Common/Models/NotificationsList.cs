namespace Kurier.Common.Models
{
    public class NotificationsList : List<string>
    {
        public Guid ClientId { get; set; }
    }
}
