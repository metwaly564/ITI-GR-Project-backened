using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    /// <summary>
    /// DTO for backup/import data structure
    /// Matches the JSON format provided by the user
    /// </summary>
    public class BackupCategoryDTO
    {
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category_name_en")]
        public string? CategoryNameEn { get; set; }

        [JsonPropertyName("category_name_ar")]
        public string? CategoryNameAr { get; set; }

        [JsonPropertyName("category_rank")]
        public int? CategoryRank { get; set; }

        [JsonPropertyName("products")]
        public List<BackupProductDTO> Products { get; set; } = new List<BackupProductDTO>();
    }

    public class BackupProductDTO
    {
        [JsonPropertyName("sku_id")]
        public string? SkuId { get; set; }

        [JsonPropertyName("product_name_en")]
        public string? ProductNameEn { get; set; }

        [JsonPropertyName("product_name_ar")]
        public string? ProductNameAr { get; set; }

        [JsonPropertyName("company_name")]
        public string? CompanyName { get; set; }

        [JsonPropertyName("product_card_description_en")]
        public string? ProductCardDescriptionEn { get; set; }

        [JsonPropertyName("product_card_description_ar")]
        public string? ProductCardDescriptionAr { get; set; }

        [JsonPropertyName("product_description_en")]
        public string? ProductDescriptionEn { get; set; }

        [JsonPropertyName("product_description_ar")]
        public string? ProductDescriptionAr { get; set; }

        [JsonPropertyName("price_before")]
        public decimal? PriceBefore { get; set; }

        [JsonPropertyName("price_after")]
        public decimal? PriceAfter { get; set; }

        [JsonPropertyName("available_stock")]
        public int? AvailableStock { get; set; }

        [JsonPropertyName("max_order_quantity")]
        public int? MaxOrderQuantity { get; set; }

        [JsonPropertyName("item_rank")]
        public int? ItemRank { get; set; }

        [JsonPropertyName("images")]
        public List<BackupImageDTO> Images { get; set; } = new List<BackupImageDTO>();
    }

    public class BackupImageDTO
    {
        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("image_rank")]
        public int? ImageRank { get; set; }
    }

    public class BackupImportResponseDTO
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("categoriesCreated")]
        public int CategoriesCreated { get; set; }

        [JsonPropertyName("categoriesUpdated")]
        public int CategoriesUpdated { get; set; }

        [JsonPropertyName("productsCreated")]
        public int ProductsCreated { get; set; }

        [JsonPropertyName("productsUpdated")]
        public int ProductsUpdated { get; set; }

        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; } = new List<string>();
    }
}
