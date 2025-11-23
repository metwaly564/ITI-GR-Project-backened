using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using Microsoft.AspNetCore.Http;

namespace ITI.Pharmacy.API.Services
{
    public interface IProductService1
    {
     
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO dto, IFormFile? image = null);
        Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO dto, IFormFile? image = null);
        Task<bool> DeleteProductAsync(int id);
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<List<ProductDTO>> GetFeaturedProductsAsync();
        Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId);

    
        Task<List<ProductDTO>> SearchProductsAsync(string searchTerm);
        Task<bool> ToggleFeaturedStatusAsync(int id);
        Task<int> GetProductsCountAsync(bool? isActive = null);
        Task<PaginationResponse<ProductDTO>> GetProductsPaginatedAsync(int page, int pageSize, string? search = null);

 
        Task<List<ProductStockDTO>> GetLowStockProductsAsync();
        Task<List<ProductStockDTO>> GetOutOfStockProductsAsync();
        Task<ProductStockDTO> UpdateStockAsync(UpdateStockDTO dto);
        Task<ProductDTO> ToggleProductActiveAsync(int id);
        Task<InventoryAnalyticsDTO> GetInventoryAnalyticsAsync();
    }
}