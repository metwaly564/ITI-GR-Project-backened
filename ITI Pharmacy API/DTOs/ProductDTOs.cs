using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    public class ProductDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("productImage")]
        public string ProductImage { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("isPrescriptionRequired")]
        public bool IsPrescriptionRequired { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("isFeatured")]
        public bool IsFeatured { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; } = string.Empty;

        [JsonPropertyName("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("rank")]
        public int? Rank { get; set; }

        [JsonPropertyName("maxOrderQuantity")]
        public int? MaxOrderQuantity { get; set; }

        [JsonPropertyName("stock")]
        public List<ProductStockDTO> Stock { get; set; } = new List<ProductStockDTO>();
    }

    public class CreateProductDTO
    {
        [JsonPropertyName("productName")]
        public required string ProductName { get; set; }

        [JsonPropertyName("productImage")]
        public required string ProductImage { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("isPrescriptionRequired")]
        public bool IsPrescriptionRequired { get; set; }

        [JsonPropertyName("isFeatured")]
        public bool IsFeatured { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; } = true;

        [JsonPropertyName("rank")]
        public int? Rank { get; set; }

        [JsonPropertyName("maxOrderQuantity")]
        public int? MaxOrderQuantity { get; set; }
    }

    public class UpdateProductDTO
    {
        [JsonPropertyName("productName")]
        public string? ProductName { get; set; }

        [JsonPropertyName("productImage")]
        public string? ProductImage { get; set; }

        [JsonPropertyName("price")]
        public decimal? Price { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("isPrescriptionRequired")]
        public bool? IsPrescriptionRequired { get; set; }

        [JsonPropertyName("isFeatured")]
        public bool? IsFeatured { get; set; }

        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; }

        [JsonPropertyName("rank")]
        public int? Rank { get; set; }

        [JsonPropertyName("maxOrderQuantity")]
        public int? MaxOrderQuantity { get; set; }
    }
}

   