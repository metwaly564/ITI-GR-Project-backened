using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrescriptionRequired { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ProductStockDTO> Stock { get; set; } = new();
    }

    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool IsPrescriptionRequired { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }

    public class UpdateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public bool? IsPrescriptionRequired { get; set; }
        public int? CategoryId { get; set; }
        public bool? IsActive { get; set; }
    }
}