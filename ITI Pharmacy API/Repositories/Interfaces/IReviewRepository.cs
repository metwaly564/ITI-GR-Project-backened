using ITI.Pharmacy.API.Models;

namespace ITI.Pharmacy.API.Repositories.Interfaces
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetByProductIdAsync(int productId);
        Task<bool> HasPurchasedProductAsync(string userId, int productId);
        Task<bool> HasAlreadyReviewedAsync(string userId, int productId);
        Task<string?> GetProductPurchaseStatusAsync(string userId, int productId);
    }
}
