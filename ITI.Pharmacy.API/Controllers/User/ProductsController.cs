using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
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
                return BadRequest("Search term is required");

            var products = await _productService.SearchProductsAsync(term);
            return Ok(products);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<ProductDTO>>> GetFeaturedProducts()
        {
            var products = await _productService.GetFeaturedProductsAsync();
            return Ok(products);
        }
    }
}
