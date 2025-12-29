using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Repositories.Interfaces;

namespace ITI.Pharmacy.API.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ReviewDTO>> GetProductReviewsAsync(int productId)
        {
            var reviews = await _unitOfWork.Reviews.GetByProductIdAsync(productId);
            var reviewDTOs = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);

            foreach (var dto in reviewDTOs)
            {
                dto.IsVerifiedPurchase = await _unitOfWork.Reviews.HasPurchasedProductAsync(dto.UserId, productId);
            }

            return reviewDTOs;
        }

        public async Task<ReviewStatusDTO> GetUserReviewStatusAsync(string userId, int productId, bool isAdmin = false)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return new ReviewStatusDTO { CanReview = false, Message = "Please login to leave a review.", IsVerifiedPurchase = false };
            }

            var hasPurchased = await _unitOfWork.Reviews.HasPurchasedProductAsync(userId, productId);
            if (!hasPurchased && !isAdmin)
            {
                var currentStatus = await _unitOfWork.Reviews.GetProductPurchaseStatusAsync(userId, productId);
                if (currentStatus != null)
                {
                    return new ReviewStatusDTO 
                    { 
                        CanReview = false, 
                        Message = $"Your order is currently '{currentStatus}'. You can leave a review once it is delivered.", 
                        IsVerifiedPurchase = false 
                    };
                }

                return new ReviewStatusDTO { CanReview = false, Message = "Only verified buyers can review this product.", IsVerifiedPurchase = false };
            }

            var hasReviewed = await _unitOfWork.Reviews.HasAlreadyReviewedAsync(userId, productId);
            if (hasReviewed)
            {
                return new ReviewStatusDTO { CanReview = false, Message = "You have already reviewed this product.", IsVerifiedPurchase = true };
            }

            return new ReviewStatusDTO { CanReview = true, Message = "You can review this product.", IsVerifiedPurchase = isAdmin || hasPurchased };
        }

        public async Task<ReviewDTO> AddReviewAsync(string userId, CreateReviewDTO dto, bool isAdmin = false)
        {
            var status = await GetUserReviewStatusAsync(userId, dto.ProductId, isAdmin);
            if (!status.CanReview)
            {
                throw new InvalidOperationException(status.Message);
            }

            var review = new Review
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.AddAsync(review);
            await _unitOfWork.SaveChangesAsync();

            // Update product average rating and count
            var product = await _unitOfWork.Products.GetByIdAsync(dto.ProductId);
            if (product != null)
            {
                var productReviews = await _unitOfWork.Reviews.GetByProductIdAsync(dto.ProductId);
                product.ReviewCount = productReviews.Count();
                product.Rating = (decimal)productReviews.Average(r => r.Rating);
                await _unitOfWork.SaveChangesAsync();
            }

            var result = _mapper.Map<ReviewDTO>(review);
            result.IsVerifiedPurchase = isAdmin || status.IsVerifiedPurchase;
            return result;
        }

        public async Task<decimal> GetProductAverageRatingAsync(int productId)
        {
            var reviews = await _unitOfWork.Reviews.GetByProductIdAsync(productId);
            if (!reviews.Any()) return 0;
            return (decimal)reviews.Average(r => r.Rating);
        }
    }
}
