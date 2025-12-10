using System.ComponentModel.DataAnnotations;
using ITI.Pharmacy.API.Models;

namespace ITI.Pharmacy.API.DTOs
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required string ImageUrl { get; set; } = string.Empty;
        public required string Link { get; set; } = string.Empty;
        public BannerType Type { get; set; } = BannerType.Main;
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
        
        // Accept type as int (0 = Main, 1 = Middle) for better FormData binding
        // FormData sends strings, so we'll accept int and convert in controller
        public int Type { get; set; } = 0;

        // ImageUrl is optional when uploading a file
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class UpdateBannerDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public BannerType? Type { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
