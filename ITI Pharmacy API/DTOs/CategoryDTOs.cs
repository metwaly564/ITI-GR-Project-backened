using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
        public int? Rank { get; set; }
    }

    public class CreateCategoryDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Rank { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public int? Rank { get; set; }
    }

    /// <summary>
    /// DTO for bulk category image URL update
    /// </summary>
    public class CategoryImageUpdateDTO
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public int? Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("nameEn")]
        public string? NameEn { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("nameAr")]
        public string? NameAr { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("ImageUrl")]
        public string? ImageUrl { get; set; }
    }

    public class CategoryImageUpdateResponseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int UpdatedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
