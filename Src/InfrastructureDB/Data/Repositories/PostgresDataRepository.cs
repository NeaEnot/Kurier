using Ardalis.Specification.EntityFrameworkCore;
using InfrastructureDB.Data.Postgres;
using Kurier.Common.Interfaces;

namespace InfrastructureDB.Data.Repositories
{
    public class PostgresDataRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
    {
        public PostgresDataRepository(PostgresDataContext dbContext) : base(dbContext)
        {
        }
    }
}
