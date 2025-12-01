using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
