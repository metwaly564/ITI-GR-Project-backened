using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.Models
{
    public enum BannerType
    {
        Main,
        Middle
    }

    public class Banner
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(200)]
        public string Link { get; set; }

        [Required]
        public BannerType Type { get; set; } = BannerType.Main;

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}