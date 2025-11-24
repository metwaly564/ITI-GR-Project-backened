using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Repositories;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Repositories
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Wishlist?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
    }
}
