using InfrastructureDB.Data.Logging;
using InfrastructureDB.Data.Postgres;
using InfrastructureDB.Data.Repositories;
using Kurier.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureDB.Data
{
    public static class DbServicesHelper
    {
        public static IServiceCollection AddDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PostgresDataContext>(_ => _.UseNpgsql(configuration.GetConnectionString("PostgresDbConnection"),
        _ => _.MigrationsHistoryTable("__EFMigrationsHistory", PostgresDataContext.SchemaName)));
            services.AddScoped(typeof(IApplicationLogger<>), typeof(LoggerAdapter<>));
            services.AddScoped(typeof(IRepository<>), typeof(PostgresDataRepository<>));
            services.AddScoped(typeof(IReadRepository<>), typeof(PostgresDataRepository<>));

            return services;
        }
    }
}
