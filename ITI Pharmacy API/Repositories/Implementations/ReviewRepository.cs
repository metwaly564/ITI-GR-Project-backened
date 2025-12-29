using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetByProductIdAsync(int productId)
        {
            return await _dbSet
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> HasPurchasedProductAsync(string userId, int productId)
        {
            return await _context.Orders
                .AnyAsync(o => o.UserId == userId && 
                               (o.Status == "Delivered" || o.Status == "Completed") &&
                               o.OrderItems.Any(oi => oi.ProductId == productId));
        }

        public async Task<bool> HasAlreadyReviewedAsync(string userId, int productId)
        {
            return await _dbSet.AnyAsync(r => r.UserId == userId && r.ProductId == productId);
        }

        public async Task<string?> GetProductPurchaseStatusAsync(string userId, int productId)
        {
            var order = await _context.Orders
                .Where(o => o.UserId == userId && o.OrderItems.Any(oi => oi.ProductId == productId))
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => o.Status)
                .FirstOrDefaultAsync();

            return order;
        }
    }
}
