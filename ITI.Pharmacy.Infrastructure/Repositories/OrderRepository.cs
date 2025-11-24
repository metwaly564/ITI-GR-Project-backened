using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Repositories;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Order?> GetWithItemsAsync(int id)
        {
            return await _dbSet
                .Include(o => o.OrderItems)
                .Include(o => o.User)
                .Include(o => o.Branch)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ToListAsync();
        }
    }
}
