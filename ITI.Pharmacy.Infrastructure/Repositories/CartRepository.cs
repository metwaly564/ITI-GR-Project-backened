using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Repositories;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Cart?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
