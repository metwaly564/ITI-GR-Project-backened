using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IRecommendationService
    {
        Task<RecommendationResponseDTO> AnalyzeAndRecommendAsync(RecommendationRequestDTO request, string? userId = null);
        Task<List<ProductRecommendationDTO>> GetRecommendationsAsync(Models.Enums.SkinType skinType, List<string> concerns);
    }
}

