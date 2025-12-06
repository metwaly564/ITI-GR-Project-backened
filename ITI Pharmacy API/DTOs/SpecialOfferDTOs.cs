using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    /// <summary>
    /// DTO for category import (category with products)
    /// </summary>
    public class CategoryImportDTO
    {
        [JsonPropertyName("nameEn")]
        public string? NameEn { get; set; }

        [JsonPropertyName("nameAr")]
        public string? NameAr { get; set; }

        [JsonPropertyName("products")]
        public List<CategoryProductDTO> Products { get; set; } = new List<CategoryProductDTO>();
    }

    public class CategoryProductDTO
    {
        [JsonPropertyName("skuId")]
        public string? SkuId { get; set; }

        [JsonPropertyName("nameEn")]
        public string? NameEn { get; set; }

        [JsonPropertyName("nameAr")]
        public string? NameAr { get; set; }

        [JsonPropertyName("company")]
        public string? Company { get; set; }

        [JsonPropertyName("priceBefore")]
        public decimal? PriceBefore { get; set; }

        [JsonPropertyName("priceAfter")]
        public decimal? PriceAfter { get; set; }

        [JsonPropertyName("availableStock")]
        public int? AvailableStock { get; set; }

        [JsonPropertyName("Images")]
        public List<CategoryProductImageDTO> Images { get; set; } = new List<CategoryProductImageDTO>();
    }

    public class CategoryProductImageDTO
    {
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class CategoryImportResponseDTO
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("categoryName")]
        public string? CategoryName { get; set; }

        [JsonPropertyName("productsCreated")]
        public int ProductsCreated { get; set; }

        [JsonPropertyName("productsUpdated")]
        public int ProductsUpdated { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new List<string>();
    }
}
