using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
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
            try
            {
                var banners = await _bannerService.GetAllBannersAsync();
                var categories = await _categoryService.GetAllCategoriesAsync();
                var featuredProducts = await _productService.GetFeaturedProductsAsync();

                // Get popular products based on total stock quantity across all branches
                var popularProducts = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductStocks)
                        .ThenInclude(ps => ps.Branch)
                    .Where(p => p.IsActive)
                    .ToListAsync();

                // Order by total stock quantity, handling null collections
                var orderedProducts = popularProducts
                    .OrderByDescending(p => p.ProductStocks != null 
                        ? p.ProductStocks.Sum(ps => ps.Quantity) 
                        : 0)
                    .Take(8)
                    .ToList();

                var popularProductsDTO = orderedProducts.Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name ?? string.Empty,
                    Description = p.Description ?? string.Empty,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    ImageUrl = p.ImageUrl ?? string.Empty,
                    IsPrescriptionRequired = p.IsPrescriptionRequired,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category?.Name ?? string.Empty,
                    CreatedAt = p.CreatedAt,
                    Stock = p.ProductStocks?.Select(ps => new ProductStockDTO
                    {
                        ProductId = ps.ProductId,
                        BranchId = ps.BranchId,
                        BranchName = ps.Branch?.Name ?? string.Empty,
                        Quantity = ps.Quantity,
                        MinimumStockLevel = ps.MinimumStockLevel,
                        MaximumStockLevel = ps.MaximumStockLevel,
                        LastRestocked = ps.LastRestocked
                    }).ToList() ?? new List<ProductStockDTO>()
                }).ToList();

                return new HomeDataDTO
                {
                    Banners = banners ?? new List<BannerDTO>(),
                    Categories = categories ?? new List<CategoryDTO>(),
                    FeaturedProducts = featuredProducts ?? new List<ProductDTO>(),
                    PopularProducts = popularProductsDTO
                };
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use ILogger here)
                throw new Exception($"Error getting home data: {ex.Message}", ex);
            }
        }
    }
}