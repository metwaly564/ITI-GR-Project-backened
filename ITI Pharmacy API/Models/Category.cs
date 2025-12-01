using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; } //  Nullable

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int? Rank { get; set; } // Category rank for sorting/display order

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
