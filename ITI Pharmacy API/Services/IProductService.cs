using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId);
        Task<List<ProductDTO>> SearchProductsAsync(string searchTerm);
        Task<ProductDTO> CreateProductAsync(CreateProductDTO dto, IFormFile image);
        Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO dto, IFormFile image);
        Task<bool> DeleteProductAsync(int id);
        Task<List<ProductDTO>> GetFeaturedProductsAsync();
        Task<List<ProductDTO>> GetProductsWithLowStockAsync(int branchId);
    }
}