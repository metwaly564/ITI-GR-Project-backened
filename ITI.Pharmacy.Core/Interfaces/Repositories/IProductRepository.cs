using ITI.Pharmacy.Core.Entities;

namespace ITI.Pharmacy.Core.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product?> GetWithCategoryAsync(int id);
    }
}
