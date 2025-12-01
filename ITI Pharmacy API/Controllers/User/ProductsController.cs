using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductsByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<ProductDTO>>> SearchProducts([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest(new { message = "Search term is required" });

            var products = await _productService.SearchProductsAsync(term);
            return Ok(products);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<ProductDTO>>> GetFeaturedProducts()
        {
            var products = await _productService.GetFeaturedProductsAsync();
            return Ok(products);
        }

        /// <summary>
        /// Advanced product query endpoint - Perfect for Angular frontend
        /// Supports: pagination, sorting, filtering by branch/category, search, price range
        /// Example: /api/Products/query?page=1&pageSize=10&sortBy=price&branchId=1&categoryId=2&search=paracetamol
        /// </summary>
        [HttpGet("query")]
        public async Task<ActionResult<PaginationResponse<ProductDTO>>> GetProductsAdvanced([FromQuery] ProductQueryDTO query)
        {
            var result = await _productService.GetProductsAdvancedAsync(query);
            return Ok(result);
        }

        /// <summary>
        /// Get products with pagination (simplified version)
        /// Example: /api/Products/paginated?page=1&pageSize=10&search=medicine
        /// </summary>
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationResponse<ProductDTO>>> GetProductsPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            var result = await _productService.GetProductsPaginatedAsync(page, pageSize, search);
            return Ok(result);
        }
    }
}
