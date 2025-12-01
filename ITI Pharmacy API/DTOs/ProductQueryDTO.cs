using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    /// <summary>
    /// Query parameters for advanced product filtering, sorting, and pagination
    /// Angular-friendly with camelCase properties
    /// </summary>
    public class ProductQueryDTO
    {
        [JsonPropertyName("page")]
        public int Page { get; set; } = 1;

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; } = 10;

        [JsonPropertyName("search")]
        public string? Search { get; set; }

        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("branchId")]
        public int? BranchId { get; set; }

        [JsonPropertyName("sortBy")]
        public string? SortBy { get; set; } // "name", "price", "priceDesc", "date", "rating"

        [JsonPropertyName("minPrice")]
        public decimal? MinPrice { get; set; }

        [JsonPropertyName("maxPrice")]
        public decimal? MaxPrice { get; set; }

        [JsonPropertyName("isPrescriptionRequired")]
        public bool? IsPrescriptionRequired { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; } = true;

        [JsonPropertyName("inStock")]
        public bool? InStock { get; set; } // Filter products that have stock in the branch
    }
}
