namespace Kurier.Common.Models
{
    public class CancelOrderRequest
    {
        public Guid ClientTokenId { get; set; }
        public Guid OrderId { get; set; }
    }
}
