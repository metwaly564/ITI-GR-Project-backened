using ITI.Pharmacy.Core.Entities;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface IWishlistRepository : IGenericRepository<Wishlist>
    {
        Task<Wishlist?> GetByUserIdAsync(string userId);
    }
}
