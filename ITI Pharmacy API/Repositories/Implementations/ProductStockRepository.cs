using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class ProductStockRepository : GenericRepository<ProductStock>, IProductStockRepository
    {
        public ProductStockRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
