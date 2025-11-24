using ITI.Pharmacy.API.DTOs;
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
            var response = await _categoryService.GetAllCategoriesAsync();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound(new { message = $"Category with id {id} not found" });

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> CreateCategory([FromForm] CreateCategoryDTO dto, [FromForm] IFormFile image)
        {
            var response = await _categoryService.CreateCategoryAsync(dto, image);
            if (response?.Data == null)
                return BadRequest(new { message = response?.Message ?? "Failed to create category" });

            return CreatedAtAction(nameof(GetCategory), new { id = response.Data.Id }, response.Data);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> UpdateCategory(int id, [FromForm] UpdateCategoryDTO dto, [FromForm] IFormFile image)
        {
            var response = await _categoryService.UpdateCategoryAsync(id, dto, image);
            if (response?.Data == null)
                return NotFound(new { message = $"Category with id {id} not found" });

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await _categoryService.DeleteCategoryAsync(id);
            if (result == null || result.Data == false)
                return NotFound(new { message = $"Category with id {id} not found" });

            return NoContent();
        }
    }
}
