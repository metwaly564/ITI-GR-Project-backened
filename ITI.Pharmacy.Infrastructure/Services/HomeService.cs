using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductService _productService;
        private readonly IBannerService _bannerService;
        private readonly ICategoryService _categoryService;

        public HomeService(ApplicationDbContext context, IProductService productService,
            IBannerService bannerService, ICategoryService categoryService)
        {
            _context = context;
            _productService = productService;
            _bannerService = bannerService;
            _categoryService = categoryService;
        }

        public async Task<HomeDataDTO> GetHomeDataAsync()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            var categories = await _categoryService.GetAllCategoriesAsync();
            var featuredProducts = await _productService.GetFeaturedProductsAsync();

         
            var popularProducts = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.ProductStocks.Sum(oi => oi.Quantity))
                .Take(8)
                .ToListAsync();

            var popularProductsDTO = popularProducts.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageUrl = p.ImageUrl,
                IsPrescriptionRequired = p.IsPrescriptionRequired,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                CreatedAt = p.CreatedAt
            }).ToList();

            return new HomeDataDTO
            {
                Banners = banners,
                Categories = categories,
                FeaturedProducts = featuredProducts,
                PopularProducts = popularProductsDTO
            };
        }
    }
}
