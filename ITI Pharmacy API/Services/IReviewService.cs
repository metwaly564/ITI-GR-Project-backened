using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId);
        Task<ReviewStatusDTO> GetUserReviewStatusAsync(string userId, int productId, bool isAdmin = false);
        Task<ReviewDTO> AddReviewAsync(string userId, CreateReviewDTO dto, bool isAdmin = false);
        Task<decimal> GetProductAverageRatingAsync(int productId);
    }
}
