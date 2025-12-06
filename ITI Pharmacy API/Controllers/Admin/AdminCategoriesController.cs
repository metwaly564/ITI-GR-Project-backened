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
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public AdminCategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        /// <summary>
        /// Bulk update category image URLs from JSON
        /// Matches categories by nameEn or nameAr, updates ImageUrl if category exists
        /// </summary>
        [HttpPost("update-image-urls")]
        public async Task<ActionResult<AdvancedApiResponse<CategoryImageUpdateResponseDTO>>> UpdateCategoryImageUrls(
            [FromBody] List<CategoryImageUpdateDTO> categories)
        {
            try
            {
                if (categories == null || !categories.Any())
                {
                    return BadRequest(new AdvancedApiResponse<CategoryImageUpdateResponseDTO>
                    {
                        Success = false,
                        Message = "Category list is required",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                var result = await _categoryService.UpdateCategoryImageUrlsAsync(categories);

                return Ok(new AdvancedApiResponse<CategoryImageUpdateResponseDTO>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result,
                    Errors = result.Errors.Any() ? result.Errors : null,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<CategoryImageUpdateResponseDTO>
                {
                    Success = false,
                    Message = "Error updating category image URLs",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromForm] CreateCategoryDTO dto, IFormFile image)
        {
            try
            {
                var category = await _categoryService.CreateCategoryAsync(dto, image);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, [FromForm] UpdateCategoryDTO dto, IFormFile image)
        {
            try
            {
                var category = await _categoryService.UpdateCategoryAsync(id, dto, image);
                if (category == null) return NotFound();
                return Ok(category);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}