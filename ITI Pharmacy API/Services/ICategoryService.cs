using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<CategoryDTO> GetCategoryByIdAsync(int id);
        Task<CategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto, IFormFile image);
        Task<CategoryDTO> UpdateCategoryAsync(int id, UpdateCategoryDTO dto, IFormFile image);
        Task<bool> DeleteCategoryAsync(int id);
        Task<CategoryImageUpdateResponseDTO> UpdateCategoryImageUrlsAsync(List<CategoryImageUpdateDTO> categories);
        Task<List<TopRankedCategoryDTO>> GetTopRankedCategoriesAsync();
    }
}