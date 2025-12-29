using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            try
            {
                var reviews = await _reviewService.GetProductReviewsAsync(productId);
                return Ok(reviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = "Error fetching product reviews", 
                    error = ex.Message,
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace 
                });
            }
        }

        [HttpGet("status/{productId}")]
        [Authorize]
        public async Task<IActionResult> GetReviewStatus(int productId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var isAdmin = User.IsInRole("Admin");
                var status = await _reviewService.GetUserReviewStatusAsync(userId, productId, isAdmin);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = "Error checking review status", 
                    error = ex.Message, 
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace 
                });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReview([FromBody] CreateReviewDTO dto)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var isAdmin = User.IsInRole("Admin");
                var result = await _reviewService.AddReviewAsync(userId, dto, isAdmin);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = "An error occurred while adding your review.", 
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message 
                });
            }
        }

        [HttpGet("debug-status/{productId}")]
        [Authorize]
        public async Task<IActionResult> GetReviewStatusDebug(int productId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId)) return Unauthorized();

                var isAdmin = User.IsInRole("Admin");
                var status = await _reviewService.GetUserReviewStatusAsync(userId, productId, isAdmin);
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = "Debug: Review status failed", 
                    error = ex.Message, 
                    inner = ex.InnerException?.Message,
                    stack = ex.StackTrace 
                });
            }
        }
    }
}
