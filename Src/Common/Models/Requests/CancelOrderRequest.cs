namespace Kurier.Common.Models.Requests
{
    public class CancelOrderRequest
    {
        public Guid ClientTokenId { get; set; }
        public Guid OrderId { get; set; }
    }
}
