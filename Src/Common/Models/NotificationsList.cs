namespace Kurier.Common.Models
{
    public class NotificationsList : List<string>
    {
        public Guid UserId { get; set; }
    }
}
