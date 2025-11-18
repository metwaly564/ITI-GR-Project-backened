namespace ITI.Pharmacy.API.DTOs
{
    public class HomeDataDTO
    {
        public List<BannerDTO> Banners { get; set; } = new();
        public List<CategoryDTO> Categories { get; set; } = new();
        public List<ProductDTO> FeaturedProducts { get; set; } = new();
        public List<ProductDTO> PopularProducts { get; set; } = new();
    }
}