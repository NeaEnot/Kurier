using Ardalis.Specification.EntityFrameworkCore;
using InfrastructureDB.Data.InMemory;
using Kurier.Common.Interfaces;

namespace InfrastructureDB.Data.Repositories
{
    public class MemoryDataRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class
    {
        public MemoryDataRepository(MemoryDataContext dbContext) : base(dbContext)
        {

        }
    }
}
