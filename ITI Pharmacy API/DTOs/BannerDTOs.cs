using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required string ImageUrl { get; set; } = string.Empty;
        public required string Link { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBannerDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public int DisplayOrder { get; set; } = 0;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class UpdateBannerDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
