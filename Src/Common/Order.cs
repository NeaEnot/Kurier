namespace Kurier.Common
{
    public class Order
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Closed { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public OrderState State { get; set; }
    }
}
