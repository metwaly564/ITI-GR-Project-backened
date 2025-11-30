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
                var userId = User?.Identity?.Name;
                var result = await _recommendationService.AnalyzeAndRecommendAsync(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Analysis failed", error = ex.Message });
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

