using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.Core.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int ProductCount { get; set; }
    }

    public class CreateCategoryDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool? IsActive { get; set; }
    }
}
