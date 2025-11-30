using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ITI.Pharmacy.API.Models.Enums;

namespace ITI.Pharmacy.API.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountPrice { get; set; }

        [StringLength(500)]
        public string? ImageUrl { get; set; }

        public bool IsPrescriptionRequired { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int CategoryId { get; set; }

        // Recommendation fields (optional - for skincare products)
        [Column(TypeName = "nvarchar(max)")]
        public string? SuitableForSkinTypes { get; set; } // JSON array of SkinType values

        [Column(TypeName = "nvarchar(max)")]
        public string? Benefits { get; set; } // JSON array of strings

        [Column(TypeName = "nvarchar(max)")]
        public string? Ingredients { get; set; } // JSON array of strings

        [Column(TypeName = "decimal(3,2)")]
        public decimal? Rating { get; set; }

        public int? ReviewCount { get; set; } = 0;

        public virtual Category Category { get; set; }
        public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
    }
}