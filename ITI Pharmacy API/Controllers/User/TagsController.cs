using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    /// <summary>
    /// Tags Controller - Provides tag information for product filtering
    /// Currently returns empty list as tags are product-specific and extracted from products
    /// Can be extended in the future if tags become a separate entity
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly IProductService _productService;

        public TagsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get all tags
        /// Currently returns empty list - tags are extracted from products on the frontend
        /// This endpoint exists to prevent 404 errors from the frontend
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TagDTO>>> GetTags()
        {
            // Return empty list for now
            // In the future, if tags become a separate entity, this can be implemented
            return Ok(new List<TagDTO>());
        }
    }
}

