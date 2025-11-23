using System.Collections.Generic;

namespace ITI.Pharmacy.API.DTOs
{
    public class HomeDataDTO
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }

        public List<ProductDTO> Products { get; set; } = new();
        public List<ProductDTO> FeaturedProducts { get; set; } = new();
        public List<ProductDTO> PopularProducts { get; set; } = new();
        public List<CategoryDTO> Categories { get; set; } = new();
        public List<BannerDTO> Banners { get; set; } = new();
    }
}
