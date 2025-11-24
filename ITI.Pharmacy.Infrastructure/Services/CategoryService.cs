using AutoMapper;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using ITI.Pharmacy.Infrastructure.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
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
                ProductCount = c.Products.Count(p => p.IsActive)
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
                ProductCount = category.Products.Count(p => p.IsActive)
            };
        }

        public async Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, IFormFile image)
        {
            var category = _mapper.Map<Category>(dto);

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
    }
}
