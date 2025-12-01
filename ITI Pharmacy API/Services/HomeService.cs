using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HomeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<HomeDataDTO> GetHomeDataAsync(int branchId = 0)
        {
            // Get all active categories
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Products)
                .ToListAsync();

            var categoriesDTO = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive,
                ProductCount = c.Products.Count(p => p.IsActive)
            }).ToList();

            // Get active banners
            var banners = await _context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();

            var bannersDTO = _mapper.Map<List<BannerDTO>>(banners);

            // Get featured products (products with high rating or special flag)
            var featuredProductsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive && p.Rating.HasValue && p.Rating.Value >= 4.0m);

            if (branchId > 0)
            {
                featuredProductsQuery = featuredProductsQuery.Where(p => 
                    p.ProductStocks.Any(s => s.BranchId == branchId && s.Quantity > 0));
            }

            var featuredProducts = await featuredProductsQuery
                .OrderByDescending(p => p.Rating)
                .ThenByDescending(p => p.ReviewCount)
                .Take(8)
                .ToListAsync();

            var featuredProductsDTO = _mapper.Map<List<ProductDTO>>(featuredProducts);

            // Get popular products (products with most stock or most reviews)
            var popularProductsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive);

            if (branchId > 0)
            {
                popularProductsQuery = popularProductsQuery.Where(p => 
                    p.ProductStocks.Any(s => s.BranchId == branchId && s.Quantity > 0));
            }

            var popularProducts = await popularProductsQuery
                .OrderByDescending(p => p.ReviewCount)
                .ThenByDescending(p => p.ProductStocks
                    .Where(s => branchId == 0 || s.BranchId == branchId)
                    .Sum(s => s.Quantity))
                .Take(8)
                .ToListAsync();

            var popularProductsDTO = _mapper.Map<List<ProductDTO>>(popularProducts);

            // Get all products (for the main product list)
            var allProductsQuery = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive);

            if (branchId > 0)
            {
                allProductsQuery = allProductsQuery.Where(p => 
                    p.ProductStocks.Any(s => s.BranchId == branchId && s.Quantity > 0));
            }

            var allProducts = await allProductsQuery
                .OrderByDescending(p => p.CreatedAt)
                .Take(20)
                .ToListAsync();

            var allProductsDTO = _mapper.Map<List<ProductDTO>>(allProducts);

            return new HomeDataDTO
            {
                Categories = categoriesDTO,
                Banners = bannersDTO,
                FeaturedProducts = featuredProductsDTO,
                PopularProducts = popularProductsDTO,
                Products = allProductsDTO,
                TotalCount = allProductsDTO.Count,
                Page = 1,
                Limit = 20
            };
        }
    }
}