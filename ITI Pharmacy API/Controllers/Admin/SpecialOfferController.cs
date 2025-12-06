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
    public class CategoryImportController : ControllerBase
    {
        private readonly ICategoryImportService _categoryImportService;

        public CategoryImportController(ICategoryImportService categoryImportService)
        {
            _categoryImportService = categoryImportService;
        }

        /// <summary>
        /// Import category with products from JSON
        /// Creates category if it doesn't exist (checked by nameEn)
        /// Creates/updates products and assigns them to the category
        /// </summary>
        [HttpPost("import")]
        public async Task<ActionResult<AdvancedApiResponse<CategoryImportResponseDTO>>> ImportCategory(
            [FromBody] CategoryImportDTO categoryImport)
        {
            try
            {
                if (categoryImport == null)
                {
                    return BadRequest(new AdvancedApiResponse<CategoryImportResponseDTO>
                    {
                        Success = false,
                        Message = "Category import data is required",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                var result = await _categoryImportService.ImportCategoryAsync(categoryImport);

                return Ok(new AdvancedApiResponse<CategoryImportResponseDTO>
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
                return StatusCode(500, new AdvancedApiResponse<CategoryImportResponseDTO>
                {
                    Success = false,
                    Message = "Error importing category",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
