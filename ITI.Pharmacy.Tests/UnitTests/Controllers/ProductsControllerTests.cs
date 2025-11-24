using ITI.Pharmacy.API.Controllers.User;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.Tests.UnitTests.Controllers;

public class ProductsControllerTests
{
    [Fact]
    public async Task GetProducts_ReturnsOkWithProducts()
    {
        var service = new FakeProductService
        {
            AllProducts = new List<ProductDTO>
            {
                new() { Id = 1, Name = "Panadol" },
                new() { Id = 2, Name = "Vitamin C" }
            }
        };

        var controller = new ProductsController(service);

        var result = await controller.GetProducts();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsAssignableFrom<List<ProductDTO>>(okResult.Value);
        Assert.Equal(2, payload.Count);
        Assert.Contains(payload, p => p.Name == "Panadol");
    }

    [Fact]
    public async Task GetProduct_WhenMissing_ReturnsNotFound()
    {
        var service = new FakeProductService { ProductById = null };
        var controller = new ProductsController(service);

        var result = await controller.GetProduct(42);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task SearchProducts_WithEmptyTerm_ReturnsBadRequest()
    {
        var controller = new ProductsController(new FakeProductService());

        var result = await controller.SearchProducts("   ");

        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Search term is required", badRequest.Value);
    }

    private sealed class FakeProductService : IProductService
    {
        public List<ProductDTO> AllProducts { get; set; } = new();
        public ProductDTO? ProductById { get; set; } = new ProductDTO { Id = 1, Name = "Default" };
        public List<ProductDTO> ProductsByCategory { get; set; } = new();
        public List<ProductDTO> SearchResults { get; set; } = new();
        public List<ProductDTO> FeaturedProducts { get; set; } = new();

        public Task<List<ProductDTO>> GetAllProductsAsync() => Task.FromResult(AllProducts);
        public Task<ProductDTO> GetProductByIdAsync(int id) =>
            Task.FromResult(ProductById ?? null!);
        public Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId) => Task.FromResult(ProductsByCategory);
        public Task<List<ProductDTO>> SearchProductsAsync(string searchTerm) => Task.FromResult(SearchResults);
        public Task<List<ProductDTO>> GetFeaturedProductsAsync() => Task.FromResult(FeaturedProducts);
        public Task<List<ProductDTO>> GetProductsWithLowStockAsync(int branchId) => Task.FromResult(new List<ProductDTO>());

        public Task<ProductDTO> CreateProductAsync(CreateProductDTO dto, IFormFile image) =>
            throw new NotImplementedException();

        public Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO dto, IFormFile image) =>
            throw new NotImplementedException();

        public Task<bool> DeleteProductAsync(int id) =>
            throw new NotImplementedException();
    }
}

