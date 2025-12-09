using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    /// <summary>
    /// Categories Controller - Provides category information for frontend
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get all active categories
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        /// <summary>
        /// Get category by ID with details
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategory(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }
    }
}

