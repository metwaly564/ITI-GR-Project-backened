using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Responses;

public interface ICategoryService
{
    Task<AdvancedApiResponse<List<CategoryDTO>>> GetAllCategoriesAsync();
    Task<AdvancedApiResponse<CategoryDTO>> GetCategoryByIdAsync(int id);
    Task<AdvancedApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO dto, IFormFile image);
    Task<AdvancedApiResponse<CategoryDTO>> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, IFormFile image);
    Task<AdvancedApiResponse<bool>> DeleteCategoryAsync(int id);
}
