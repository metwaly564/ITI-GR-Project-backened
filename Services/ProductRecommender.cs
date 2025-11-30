using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Enums;
using System.Text.Json;

namespace ITI.Pharmacy.API.Services
{
    public class ProductRecommender
    {
        public List<Product> RecommendProducts(List<Product> products, SkinType skinType, List<string> concerns)
        {
            var scoredProducts = products.Select(p => new
            {
                Product = p,
                Score = CalculateProductScore(p, skinType, concerns)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(10)
            .Select(x => x.Product)
            .ToList();

            return scoredProducts;
        }

        private int CalculateProductScore(Product product, SkinType skinType, List<string> concerns)
        {
            int score = 0;

            // Check if product is suitable for skin type
            var suitableSkinTypes = GetSuitableSkinTypes(product);
            if (suitableSkinTypes.Contains(skinType))
                score += 30;

            // Check if product benefits match concerns
            var benefits = GetBenefits(product);
            foreach (var concern in concerns)
            {
                if (benefits.Any(b => b.Contains(concern, StringComparison.OrdinalIgnoreCase)))
                    score += 20;
            }

            // Add rating score
            if (product.Rating.HasValue)
                score += (int)(product.Rating.Value * 2);

            // Penalize if not suitable for skin type
            if (!suitableSkinTypes.Contains(skinType))
                score -= 50;

            return Math.Max(0, score);
        }

        private List<SkinType> GetSuitableSkinTypes(Product product)
        {
            if (string.IsNullOrEmpty(product.SuitableForSkinTypes))
                return new List<SkinType>();

            try
            {
                return JsonSerializer.Deserialize<List<SkinType>>(product.SuitableForSkinTypes) ?? new List<SkinType>();
            }
            catch
            {
                return new List<SkinType>();
            }
        }

        private List<string> GetBenefits(Product product)
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

        public List<string> GetIngredients(Product product)
        {
            if (string.IsNullOrEmpty(product.Ingredients))
                return new List<string>();

            try
            {
                return JsonSerializer.Deserialize<List<string>>(product.Ingredients) ?? new List<string>();
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}

