using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ITI.Pharmacy.API.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductRecommender _productRecommender;

        public RecommendationService(ApplicationDbContext context, ProductRecommender productRecommender)
        {
            _context = context;
            _productRecommender = productRecommender;
        }

        public async Task<RecommendationResponseDTO> AnalyzeAndRecommendAsync(RecommendationRequestDTO request, string? userId = null)
        {
            // Analyze symptoms and identify conditions
            var analysisResult = AnalyzeSymptoms(request.Symptoms, request.SkinType);

            // Get recommended products
            var allProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToListAsync();

            var recommendedProducts = _productRecommender.RecommendProducts(allProducts, request.SkinType, request.Concerns);

            // Map to DTOs
            var productDTOs = recommendedProducts.Select(p => new ProductRecommendationDTO
            {
                ProductId = p.Id,
                Name = p.Name,
                Description = p.Description,
                CategoryName = p.Category?.Name ?? "",
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageUrl = p.ImageUrl ?? "",
                RecommendationReason = $"Suitable for {request.SkinType} skin and addresses {string.Join(", ", request.Concerns.Take(2))}",
                KeyIngredients = _productRecommender.GetIngredients(p).Take(5).ToList(),
                Benefits = GetBenefitsList(p)
            }).ToList();

            return new RecommendationResponseDTO
            {
                AnalysisId = Guid.NewGuid().ToString(),
                SkinType = request.SkinType,
                IdentifiedConditions = analysisResult.IdentifiedConditions,
                AnalysisSummary = analysisResult.Summary,
                RecommendedIngredients = analysisResult.RecommendedIngredients,
                IngredientsToAvoid = analysisResult.IngredientsToAvoid,
                RecommendedProducts = productDTOs,
                RoutineAdvice = analysisResult.RoutineAdvice,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<List<ProductRecommendationDTO>> GetRecommendationsAsync(SkinType skinType, List<string> concerns)
        {
            var allProducts = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToListAsync();

            var recommendedProducts = _productRecommender.RecommendProducts(allProducts, skinType, concerns);

            return recommendedProducts.Select(p => new ProductRecommendationDTO
            {
                ProductId = p.Id,
                Name = p.Name,
                Description = p.Description,
                CategoryName = p.Category?.Name ?? "",
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageUrl = p.ImageUrl ?? "",
                RecommendationReason = $"Suitable for {skinType} skin and addresses {string.Join(", ", concerns.Take(2))}",
                KeyIngredients = _productRecommender.GetIngredients(p).Take(5).ToList(),
                Benefits = GetBenefitsList(p)
            }).ToList();
        }

        private SymptomAnalysisResult AnalyzeSymptoms(List<string> symptoms, SkinType skinType)
        {
            var conditions = new List<string>();
            var recommendedIngredients = new List<string>();
            var avoidIngredients = new List<string>();

            // Simple rule-based analysis
            if (symptoms.Any(s => s.Contains("acne", StringComparison.OrdinalIgnoreCase) ||
                                 s.Contains("pimple", StringComparison.OrdinalIgnoreCase)))
            {
                conditions.Add("Acne");
                recommendedIngredients.AddRange(new[] { "Salicylic Acid", "Niacinamide", "Benzoyl Peroxide" });
                avoidIngredients.AddRange(new[] { "Heavy Oils", "Comedogenic Ingredients" });
            }

            if (symptoms.Any(s => s.Contains("dry", StringComparison.OrdinalIgnoreCase) ||
                                 s.Contains("flaky", StringComparison.OrdinalIgnoreCase)))
            {
                conditions.Add("Dry Skin");
                recommendedIngredients.AddRange(new[] { "Hyaluronic Acid", "Ceramides", "Glycerin" });
            }

            if (symptoms.Any(s => s.Contains("redness", StringComparison.OrdinalIgnoreCase) ||
                                 s.Contains("sensitive", StringComparison.OrdinalIgnoreCase)))
            {
                conditions.Add("Sensitive Skin");
                recommendedIngredients.AddRange(new[] { "Centella Asiatica", "Niacinamide", "Aloe Vera" });
                avoidIngredients.AddRange(new[] { "Alcohol", "Fragrance", "Essential Oils" });
            }

            if (symptoms.Any(s => s.Contains("oily", StringComparison.OrdinalIgnoreCase) ||
                                 s.Contains("greasy", StringComparison.OrdinalIgnoreCase)))
            {
                conditions.Add("Oily Skin");
                recommendedIngredients.AddRange(new[] { "Niacinamide", "Salicylic Acid", "Clay" });
            }

            return new SymptomAnalysisResult
            {
                IdentifiedConditions = conditions.Distinct().ToList(),
                Summary = $"Based on your symptoms, we've identified {conditions.Count} potential skin concerns. " +
                         $"Your {skinType} skin would benefit from a gentle yet effective routine.",
                RecommendedIngredients = recommendedIngredients.Distinct().ToList(),
                IngredientsToAvoid = avoidIngredients.Distinct().ToList(),
                RoutineAdvice = GetRoutineAdvice(skinType, conditions)
            };
        }

        private string GetRoutineAdvice(SkinType skinType, List<string> conditions)
        {
            var advice = new List<string>
            {
                "• Cleanse twice daily with a gentle cleanser",
                "• Apply moisturizer while skin is still damp",
                "• Use sunscreen every morning"
            };

            if (conditions.Contains("Acne"))
            {
                advice.Add("• Use acne treatments in the evening");
                advice.Add("• Avoid picking or squeezing blemishes");
            }

            if (skinType == SkinType.Dry)
            {
                advice.Add("• Consider using a hydrating serum before moisturizer");
            }

            if (skinType == SkinType.Oily)
            {
                advice.Add("• Use oil-free products");
                advice.Add("• Don't over-cleanse as it can increase oil production");
            }

            return string.Join("\n", advice);
        }

        private List<string> GetBenefitsList(Product product)
        {
            if (string.IsNullOrEmpty(product.Benefits))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(product.Benefits) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }

        private class SymptomAnalysisResult
        {
            public List<string> IdentifiedConditions { get; set; } = new();
            public string Summary { get; set; } = string.Empty;
            public List<string> RecommendedIngredients { get; set; } = new();
            public List<string> IngredientsToAvoid { get; set; } = new();
            public string RoutineAdvice { get; set; } = string.Empty;
        }
    }
}

