using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace ITI.Pharmacy.API.Services
{
    public interface IProductService
    {
        Task<ProductDTO?> GetProductByIdAsync(int id);
        Task<ProductDTO?> CreateProductAsync(CreateProductDTO dto, IFormFile? image);
        Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto, IFormFile? image);
        Task<bool> DeleteProductAsync(int id);
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<List<ProductDTO>> GetFeaturedProductsAsync();
        Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<PaginationResponse<ProductDTO>> GetProductsPaginatedAsync(int page, int pageSize, string? search = null);
        Task<List<ProductDTO>> SearchProductsAsync(string searchTerm, int? categoryId = null);
        Task<bool> ToggleFeaturedStatusAsync(int id);
        Task<int> GetProductsCountAsync(bool? isActive = null);
    }
}