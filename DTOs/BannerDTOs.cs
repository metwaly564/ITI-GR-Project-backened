using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class BannerDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateBannerDTO
    {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int DisplayOrder { get; set; } = 0;
    }

    public class UpdateBannerDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int? DisplayOrder { get; set; }
        public bool? IsActive { get; set; }
    }
}