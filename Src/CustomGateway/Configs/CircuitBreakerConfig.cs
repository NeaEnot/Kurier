namespace Kurier.CustomGateway.Configs
{
    public class CircuitBreakerConfig
    {
        public int MaxFailures { get; set; }
        public int BreakDuration { get; set; }
    }
}
