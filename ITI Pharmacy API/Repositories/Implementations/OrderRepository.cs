using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
