using ITI.Pharmacy.Core.Entities;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetWithItemsAsync(int id);
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
    }
}
