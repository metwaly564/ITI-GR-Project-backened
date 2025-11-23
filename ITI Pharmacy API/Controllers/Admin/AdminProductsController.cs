using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public AdminProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<AdvancedApiResponse<List<ProductDTO>>>> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(new AdvancedApiResponse<List<ProductDTO>>
                {
                    Success = true,
                    Message = "Products retrieved successfully",
                    Data = products,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<List<ProductDTO>>
                {
                    Success = false,
                    Message = "Error retrieving products",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdvancedApiResponse<ProductDTO>>> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new AdvancedApiResponse<ProductDTO>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new AdvancedApiResponse<ProductDTO>
                {
                    Success = true,
                    Message = "Product retrieved successfully",
                    Data = product,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Error retrieving product",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<AdvancedApiResponse<ProductDTO>>> CreateProduct([FromForm] CreateProductDTO dto, IFormFile? image)
        {
            try
            {
                // ?????? ?? ????????
                if (dto == null)
                {
                    return BadRequest(new AdvancedApiResponse<ProductDTO>
                    {
                        Success = false,
                        Message = "Product data is required",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                var product = await _productService.CreateProductAsync(dto, image);
                if (product == null)
                {
                    return BadRequest(new AdvancedApiResponse<ProductDTO>
                    {
                        Success = false,
                        Message = "Failed to create product",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new AdvancedApiResponse<ProductDTO>
                {
                    Success = true,
                    Message = "Product created successfully",
                    Data = product,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Error creating product",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AdvancedApiResponse<ProductDTO>>> UpdateProduct(int id, [FromForm] UpdateProductDTO dto, IFormFile? image)
        {
            try
            {
                var product = await _productService.UpdateProductAsync(id, dto, image);
                if (product == null)
                {
                    return NotFound(new AdvancedApiResponse<ProductDTO>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new AdvancedApiResponse<ProductDTO>
                {
                    Success = true,
                    Message = "Product updated successfully",
                    Data = product,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<ProductDTO>
                {
                    Success = false,
                    Message = "Error updating product",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AdvancedApiResponse<bool>>> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (!result)
                {
                    return NotFound(new AdvancedApiResponse<bool>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = false,
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new AdvancedApiResponse<bool>
                {
                    Success = true,
                    Message = "Product deleted successfully",
                    Data = true,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<bool>
                {
                    Success = false,
                    Message = "Error deleting product",
                    Errors = new List<string> { ex.Message },
                    Data = false,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("paginated")]
        public async Task<ActionResult<AdvancedApiResponse<PaginationResponse<ProductDTO>>>> GetProductsPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null)
        {
            try
            {
                var result = await _productService.GetProductsPaginatedAsync(page, pageSize, search);
                return Ok(new AdvancedApiResponse<PaginationResponse<ProductDTO>>
                {
                    Success = true,
                    Message = "Products retrieved successfully",
                    Data = result,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<PaginationResponse<ProductDTO>>
                {
                    Success = false,
                    Message = "Error retrieving products",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("featured")]
        public async Task<ActionResult<AdvancedApiResponse<List<ProductDTO>>>> GetFeaturedProducts()
        {
            try
            {
                var products = await _productService.GetFeaturedProductsAsync();
                return Ok(new AdvancedApiResponse<List<ProductDTO>>
                {
                    Success = true,
                    Message = "Featured products retrieved successfully",
                    Data = products,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<List<ProductDTO>>
                {
                    Success = false,
                    Message = "Error retrieving featured products",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPatch("{id}/toggle-featured")]
        public async Task<ActionResult<AdvancedApiResponse<bool>>> ToggleFeaturedStatus(int id)
        {
            try
            {
                var result = await _productService.ToggleFeaturedStatusAsync(id);
                if (!result)
                {
                    return NotFound(new AdvancedApiResponse<bool>
                    {
                        Success = false,
                        Message = "Product not found",
                        Data = false,
                        Timestamp = DateTime.UtcNow
                    });
                }

                return Ok(new AdvancedApiResponse<bool>
                {
                    Success = true,
                    Message = "Product featured status updated successfully",
                    Data = true,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<bool>
                {
                    Success = false,
                    Message = "Error updating featured status",
                    Errors = new List<string> { ex.Message },
                    Data = false,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("stats")]
        public async Task<ActionResult<AdvancedApiResponse<object>>> GetProductsStats()
        {
            try
            {
                var totalProducts = await _productService.GetProductsCountAsync();
                var activeProducts = await _productService.GetProductsCountAsync(true);
                var inactiveProducts = await _productService.GetProductsCountAsync(false);

                return Ok(new AdvancedApiResponse<object>
                {
                    Success = true,
                    Message = "Products statistics retrieved successfully",
                    Data = new
                    {
                        TotalProducts = totalProducts,
                        ActiveProducts = activeProducts,
                        InactiveProducts = inactiveProducts
                    },
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving products statistics",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}