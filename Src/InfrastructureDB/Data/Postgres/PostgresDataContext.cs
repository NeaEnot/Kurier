using InfrastructureDB.Data.Interfaces;
using Kurier.Common.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InfrastructureDB.Data.Postgres
{
    public class PostgresDataContext : DbContext, IKurierDbContext
    {
        public PostgresDataContext()
        {
        }

        public PostgresDataContext(DbContextOptions<PostgresDataContext> options) : base(options)
        {
        }

        public readonly static string SchemaName = "kurier";
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("User ID=db_admin;Password=12345;Server=localhost;Port=5431;Database=postgres_db");
            }

            optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SchemaName);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.SeedDataAsync();
        }
    }
}
