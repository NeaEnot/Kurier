using Kurier.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureDB.Data.Interfaces
{
    public interface IKurierDbContext
    {
        DbSet<Order> Orders { get; set; }
        DbSet<User> Users { get; set; }
    }
}
