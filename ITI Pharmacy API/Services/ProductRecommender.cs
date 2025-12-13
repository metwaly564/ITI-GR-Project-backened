using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Enums;
using System.Text.Json;

namespace ITI.Pharmacy.API.Services
{
    public class ProductRecommender
    {
        public List<Product> RecommendProducts(List<Product> products, SkinType skinType, List<string> concerns)
        {
            if (products == null || !products.Any())
                return new List<Product>();

            var scoredProducts = products.Select(p => new
            {
                Product = p,
                Score = CalculateProductScore(p, skinType, concerns)
            })
            .OrderByDescending(x => x.Score)
            .Take(12) // Take more products to ensure we have recommendations
            .Select(x => x.Product)
            .ToList();

            // If no products scored high enough, return top products anyway (at least 5)
            if (scoredProducts.Count == 0 && products.Any())
            {
                return products.Take(10).ToList();
            }

            return scoredProducts;
        }

        private int CalculateProductScore(Product product, SkinType skinType, List<string> concerns)
        {
            int score = 0;

            // Check if product is suitable for skin type
            var suitableSkinTypes = GetSuitableSkinTypes(product);
            
            // If SuitableForSkinTypes is not set, give a base score (more lenient)
            if (suitableSkinTypes.Count == 0)
            {
                // No skin type data - give base score so product can still be recommended
                score += 20;
            }
            else if (suitableSkinTypes.Contains(skinType))
            {
                // Perfect match - high score
                score += 50;
            }
            else
            {
                // Not suitable for this skin type - but don't completely exclude
                // Give a small score in case no other products match
                score += 5;
            }

            // Check if product benefits match concerns
            var benefits = GetBenefits(product);
            if (concerns != null && concerns.Count > 0)
            {
                foreach (var concern in concerns)
                {
                    if (benefits.Any(b => !string.IsNullOrWhiteSpace(b) && 
                        b.Contains(concern, StringComparison.OrdinalIgnoreCase)))
                        score += 25;
                }
            }
            else
            {
                // No specific concerns - give base score for any product
                score += 10;
            }

            // Add rating score (if available)
            if (product.Rating.HasValue && product.Rating.Value > 0)
                score += (int)(product.Rating.Value * 3);

            // Base score for all active products
            score += 10;

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
