namespace Kurier.Common.Interfaces
{
    public interface ICourierStorage
    {
        Task AddCourier(Guid courierId);
        Task<List<Guid>> GetCouriers();
        Task DeleteCourier(Guid courierId);
    }
}
