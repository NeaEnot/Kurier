using Kurier.Common.Models.Entities;

namespace InfrastructureDB.Data.Seed
{
    public class SeedData
    {
        public List<User> Users { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
    }
}
