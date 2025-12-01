using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class WishlistRepository : GenericRepository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
