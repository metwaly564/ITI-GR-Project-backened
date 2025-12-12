using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    public class TopRankedCategoryDTO
    {
        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }

        [JsonPropertyName("category_name_en")]
        public string CategoryNameEn { get; set; } = string.Empty;

        [JsonPropertyName("category_name_ar")]
        public string CategoryNameAr { get; set; } = string.Empty;

        [JsonPropertyName("category_rank")]
        public int? CategoryRank { get; set; }

        [JsonPropertyName("products")]
        public List<TopRankedProductDTO> Products { get; set; } = new List<TopRankedProductDTO>();
    }

    public class TopRankedProductDTO
    {
        [JsonPropertyName("sku_id")]
        public int SkuId { get; set; }

        [JsonPropertyName("product_name_en")]
        public string ProductNameEn { get; set; } = string.Empty;

        [JsonPropertyName("product_name_ar")]
        public string ProductNameAr { get; set; } = string.Empty;

        [JsonPropertyName("price_after")]
        public decimal PriceAfter { get; set; }

        [JsonPropertyName("price_before")]
        public decimal? PriceBefore { get; set; }

        [JsonPropertyName("available_stock")]
        public int AvailableStock { get; set; }

        [JsonPropertyName("item_rank")]
        public int? ItemRank { get; set; }

        [JsonPropertyName("images")]
        public List<ProductImageDTO> Images { get; set; } = new List<ProductImageDTO>();

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("category_id")]
        public int CategoryId { get; set; }
    }

    public class ProductImageDTO
    {
        [JsonPropertyName("image_url")]
        public string ImageUrl { get; set; } = string.Empty;

        [JsonPropertyName("image_rank")]
        public int ImageRank { get; set; } = 1;
    }
}

