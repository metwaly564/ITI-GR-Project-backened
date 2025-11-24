using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Utilities;
using ITI_Pharmacy_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<AdvancedApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories
                .Include(c => c.Products)
                .Where(c => c.IsActive)
                .ToListAsync();

            var dtos = categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive,
                ProductCount = c.Products.Count(p => p.IsActive)
            }).ToList();

            return AdvancedApiResponse<List<CategoryDTO>>.CreateSuccess(dtos);
        }

        public async Task<AdvancedApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (category == null)
                return AdvancedApiResponse<CategoryDTO>.CreateNotFound("Category");

            var dto = new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                ProductCount = category.Products.Count(p => p.IsActive)
            };

            return AdvancedApiResponse<CategoryDTO>.CreateSuccess(dto);
        }

        public async Task<AdvancedApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO dto, IFormFile? image)
        {
            try
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
            catch (Exception ex)
            {
                return AdvancedApiResponse<CategoryDTO>.CreateError($"Failed to create category: {ex.Message}");
            }
        }

        public async Task<AdvancedApiResponse<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, IFormFile? image)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return AdvancedApiResponse<CategoryDTO>.CreateNotFound("Category");

                if (!string.IsNullOrEmpty(dto.Name)) category.Name = dto.Name;
                if (!string.IsNullOrEmpty(dto.Description)) category.Description = dto.Description;
                if (dto.IsActive.HasValue) category.IsActive = dto.IsActive.Value;

                if (image != null)
                {
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
            catch (Exception ex)
            {
                return AdvancedApiResponse<CategoryDTO>.CreateError($"Failed to update category: {ex.Message}");
            }
        }

        public async Task<AdvancedApiResponse<bool>> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                    return AdvancedApiResponse<bool>.CreateNotFound("Category");

                category.IsActive = false;
                await _context.SaveChangesAsync();

                return AdvancedApiResponse<bool>.CreateSuccess(true, "Category deactivated successfully");
            }
            catch (Exception ex)
            {
                return AdvancedApiResponse<bool>.CreateError($"Failed to delete category: {ex.Message}");
            }
        }
    }
}
