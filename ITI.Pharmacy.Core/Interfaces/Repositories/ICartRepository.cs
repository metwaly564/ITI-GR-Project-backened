using ITI.Pharmacy.Core.Entities;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Task<Cart?> GetByUserIdAsync(string userId);
    }
}
