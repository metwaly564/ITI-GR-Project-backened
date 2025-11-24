using ITI.Pharmacy.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, IFormFile image);
        Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, IFormFile image);
        Task<bool> DeleteCategoryAsync(int id);
    }
}
