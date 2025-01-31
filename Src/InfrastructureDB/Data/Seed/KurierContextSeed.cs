using InfrastructureDB.Data.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureDB.Data.Seed
{
    public class KurierContextSeed
    {
        public static void SeedAsync(IConfiguration configuration, IServiceScope scope)
        {
            if (!UseOnlyInMemoryDatabase(configuration))
            {
                PostgresDataContextSeed(scope);
            }
        }

        private static bool UseOnlyInMemoryDatabase(IConfiguration configuration)
        {
            bool useOnlyInMemoryDatabase = false;

            if (configuration["UseOnlyInMemoryDatabase"] != null)
            {
                useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);
            }

            return useOnlyInMemoryDatabase;
        }

        private static void PostgresDataContextSeed(IServiceScope scope)
        {
            var context = scope.ServiceProvider.GetRequiredService<PostgresDataContext>();

            if (context.Database.IsNpgsql() && context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}
