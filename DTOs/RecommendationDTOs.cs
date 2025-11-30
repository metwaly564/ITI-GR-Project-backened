using ITI.Pharmacy.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class RecommendationRequestDTO
    {
        [Required]
        public SkinType SkinType { get; set; }
        public List<string> Concerns { get; set; } = new();
        public List<string> Symptoms { get; set; } = new();
        public string? AdditionalNotes { get; set; }
    }

    public class RecommendationResponseDTO
    {
        public string AnalysisId { get; set; } = string.Empty;
        public SkinType SkinType { get; set; }
        public List<string> IdentifiedConditions { get; set; } = new();
        public string AnalysisSummary { get; set; } = string.Empty;
        public List<string> RecommendedIngredients { get; set; } = new();
        public List<string> IngredientsToAvoid { get; set; } = new();
        public List<ProductRecommendationDTO> RecommendedProducts { get; set; } = new();
        public string RoutineAdvice { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class ProductRecommendationDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string RecommendationReason { get; set; } = string.Empty;
        public List<string> KeyIngredients { get; set; } = new();
        public List<string> Benefits { get; set; } = new();
    }
}

