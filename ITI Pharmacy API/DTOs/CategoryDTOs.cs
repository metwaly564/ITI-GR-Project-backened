using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ITI_Pharmacy_API.DTOs
{
    public class CategoryDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

        [JsonPropertyName("imageUrl")]
        public required string ImageUrl { get; set; }

        [JsonPropertyName("isActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("productCount")]
        public int ProductCount { get; set; }
    }

    public class CreateCategoryDTO
    {
        [Required]
        [JsonPropertyName("name")]
        public required string Name { get; set; }

        [JsonPropertyName("description")]
        public required string Description { get; set; }

    }

    public class UpdateCategoryDTO
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("isActive")]
        public bool? IsActive { get; set; }

    }
}
