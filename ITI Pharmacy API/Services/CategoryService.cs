using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public CategoryService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.IsActive)
                .ToListAsync();

            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive,
                ProductCount = c.Products.Count(p => p.IsActive),
                Rank = c.Rank
            }).ToList();
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (category == null) return null;

            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count(p => p.IsActive),
                Rank = category.Rank
            };
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, IFormFile image)
        {
            var category = _mapper.Map<Category>(dto);
            category.Rank = dto.Rank;

            if (image != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "categories");
                category.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
            }

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, IFormFile image)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) category.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description)) category.Description = dto.Description;
            if (dto.IsActive.HasValue) category.IsActive = dto.IsActive.Value;
            if (dto.Rank.HasValue) category.Rank = dto.Rank.Value;

            if (image != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, "images", "categories", category.ImageUrl);
                    FileHelper.DeleteFile(oldImagePath);
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "categories");
                category.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
            }

            await _context.SaveChangesAsync();
            return await GetCategoryByIdAsync(category.Id);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            category.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CategoryImageUpdateResponseDTO> UpdateCategoryImageUrlsAsync(List<CategoryImageUpdateDTO> categories)
        {
            var response = new CategoryImageUpdateResponseDTO
            {
                Success = true,
                UpdatedCount = 0,
                SkippedCount = 0,
                Errors = new List<string>()
            };

            if (categories == null || !categories.Any())
            {
                response.Message = "No categories provided";
                response.Success = false;
                return response;
            }

            // Load all existing categories for matching
            var existingCategories = await _context.Categories.ToListAsync();

            foreach (var categoryUpdate in categories)
            {
                try
                {
                    // Skip if no image URL provided
                    if (string.IsNullOrWhiteSpace(categoryUpdate.ImageUrl))
                    {
                        response.SkippedCount++;
                        continue;
                    }

                    Category? category = null;

                    // Try to find by ID first if provided
                    if (categoryUpdate.Id.HasValue)
                    {
                        category = existingCategories.FirstOrDefault(c => c.Id == categoryUpdate.Id.Value);
                    }

                    // If not found by ID, try to find by name (match with nameEn or nameAr)
                    if (category == null)
                    {
                        if (!string.IsNullOrWhiteSpace(categoryUpdate.NameEn))
                        {
                            category = existingCategories.FirstOrDefault(c => 
                                c.Name.Equals(categoryUpdate.NameEn, StringComparison.OrdinalIgnoreCase));
                        }

                        if (category == null && !string.IsNullOrWhiteSpace(categoryUpdate.NameAr))
                        {
                            category = existingCategories.FirstOrDefault(c => 
                                c.Name.Equals(categoryUpdate.NameAr, StringComparison.OrdinalIgnoreCase));
                        }
                    }

                    if (category != null)
                    {
                        // Update the image URL
                        category.ImageUrl = categoryUpdate.ImageUrl.Trim();
                        response.UpdatedCount++;
                    }
                    else
                    {
                        response.SkippedCount++;
                        var categoryName = categoryUpdate.NameEn ?? categoryUpdate.NameAr ?? "Unknown";
                        response.Errors.Add($"Category '{categoryName}' not found and was skipped");
                    }
                }
                catch (Exception ex)
                {
                    var categoryName = categoryUpdate.NameEn ?? categoryUpdate.NameAr ?? "Unknown";
                    response.Errors.Add($"Error updating category '{categoryName}': {ex.Message}");
                }
            }

            // Save all changes
            if (response.UpdatedCount > 0)
            {
                await _context.SaveChangesAsync();
            }

            response.Message = $"Update completed. Updated: {response.UpdatedCount} categories, Skipped: {response.SkippedCount} categories.";
            response.Success = response.UpdatedCount > 0 || response.SkippedCount > 0;

            return response;
        }

        public async Task<List<TopRankedCategoryDTO>> GetTopRankedCategoriesAsync()
        {
            // Get top 10 categories sorted by Rank (ascending)
            // Include must come before Take() in EF Core
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .Include(c => c.Products)
                    .ThenInclude(p => p.ProductStocks)
                .OrderBy(c => c.Rank ?? int.MaxValue)
                .ThenBy(c => c.Id)
                .Take(10)
                .ToListAsync();

            var result = new List<TopRankedCategoryDTO>();

            foreach (var category in categories)
            {
                // Get top 10 products in stock, sorted by Rank (ascending)
                var productsInStock = category.Products
                    .Where(p => p.IsActive && p.ProductStocks.Sum(ps => ps.Quantity) > 0)
                    .OrderBy(p => p.Rank ?? int.MaxValue)
                    .ThenBy(p => p.Id)
                    .Take(10)
                    .Select(p => new TopRankedProductDTO
                    {
                        SkuId = p.Id,
                        ProductNameEn = p.Name,
                        ProductNameAr = p.Name, // If you have Arabic names, update this
                        PriceAfter = p.DiscountPrice ?? p.Price,
                        PriceBefore = p.DiscountPrice.HasValue ? p.Price : null,
                        AvailableStock = p.ProductStocks.Sum(ps => ps.Quantity),
                        ItemRank = p.Rank,
                        Description = p.Description,
                        CategoryId = category.Id,
                        Images = string.IsNullOrEmpty(p.ImageUrl)
                            ? new List<ProductImageDTO>()
                            : new List<ProductImageDTO>
                            {
                                new ProductImageDTO
                                {
                                    ImageUrl = p.ImageUrl,
                                    ImageRank = 1
                                }
                            }
                    })
                    .ToList();

                // Only add category if it has products in stock
                if (productsInStock.Any())
                {
                    result.Add(new TopRankedCategoryDTO
                    {
                        CategoryId = category.Id,
                        CategoryNameEn = category.Name,
                        CategoryNameAr = category.Name, // If you have Arabic names, update this
                        CategoryRank = category.Rank,
                        Products = productsInStock
                    });
                }
            }

            return result;
        }
    }
}