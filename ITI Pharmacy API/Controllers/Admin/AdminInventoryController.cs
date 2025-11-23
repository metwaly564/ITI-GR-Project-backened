using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminInventoryController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<AdminInventoryController> _logger;

        public AdminInventoryController(IProductService productService, ILogger<AdminInventoryController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("products")]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("products/{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound("Product not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting product by ID: {ProductId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("products")]
        public async Task<ActionResult<ProductDTO>> CreateProduct(CreateProductDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Product data is required");

                var product = await _productService.CreateProductAsync(dto);
                return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("products/{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct(int id, UpdateProductDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Product data is required");

                var product = await _productService.UpdateProductAsync(id, dto);
                if (product == null)
                    return NotFound("Product not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product: {ProductId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                    return NotFound("Product not found");

                return Ok(new { message = "Product deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product: {ProductId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPatch("products/{id}/toggle-active")]
        public async Task<ActionResult<ProductDTO>> ToggleProductActive(int id)
        {
            try
            {
                var product = await _productService.ToggleProductActiveAsync(id);
                if (product == null)
                    return NotFound("Product not found");

                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling product active status: {ProductId}", id);
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<List<ProductStockDTO>>> GetLowStockProducts()
        {
            try
            {
                var lowStockProducts = await _productService.GetLowStockProductsAsync();
                return Ok(lowStockProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting low stock products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("out-of-stock")]
        public async Task<ActionResult<List<ProductStockDTO>>> GetOutOfStockProducts()
        {
            try
            {
                var outOfStockProducts = await _productService.GetOutOfStockProductsAsync();
                return Ok(outOfStockProducts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting out of stock products");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("stock/update")]
        public async Task<ActionResult<ProductStockDTO>> UpdateStock(UpdateStockDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Stock data is required");

                var stock = await _productService.UpdateStockAsync(dto);
                return Ok(stock);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating stock");
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("analytics")]
        public async Task<ActionResult<InventoryAnalyticsDTO>> GetInventoryAnalytics()
        {
            try
            {
                var analytics = await _productService.GetInventoryAnalyticsAsync();
                return Ok(analytics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting inventory analytics");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}