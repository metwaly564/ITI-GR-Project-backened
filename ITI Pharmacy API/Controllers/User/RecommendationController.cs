using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Enums;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        public RecommendationController(IRecommendationService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        [HttpPost("analyze")]
        [AllowAnonymous]
        public async Task<ActionResult<RecommendationResponseDTO>> AnalyzeAndRecommend(
            [FromBody] RecommendationRequestDTO request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Request body is required", error = "Request cannot be null" });
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return BadRequest(new { message = "Validation failed", errors = errors });
                }

                var userId = User?.Identity?.Name;
                var result = await _recommendationService.AnalyzeAndRecommendAsync(request, userId);
                
                if (result == null)
                {
                    return BadRequest(new { message = "Analysis failed", error = "Service returned null result" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Error in AnalyzeAndRecommend: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                
                return BadRequest(new { message = "Analysis failed", error = ex.Message, details = ex.ToString() });
            }
        }

        [HttpGet("recommendations")]
        [AllowAnonymous]
        public async Task<ActionResult<List<ProductRecommendationDTO>>> GetRecommendations(
            [FromQuery] SkinType skinType,
            [FromQuery] List<string> concerns)
        {
            try
            {
                var result = await _recommendationService.GetRecommendationsAsync(skinType, concerns ?? new List<string>());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Failed to generate recommendations", error = ex.Message });
            }
        }
    }
}
