using Ardalis.Specification;

namespace Kurier.Common.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class
    {
    }
}
