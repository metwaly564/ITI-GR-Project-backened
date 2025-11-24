using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminBannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public AdminBannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBanners()
        {
            var response = await _bannerService.GetAllBannersAsync();
            if (response?.Data == null || response.Data.Count == 0)
                return NotFound(new { message = response?.Message ?? "No banners found" });

            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBanner(int id)
        {
            var response = await _bannerService.GetBannerByIdAsync(id);
            if (response?.Data == null)
                return NotFound(new { message = response?.Message ?? $"Banner with id {id} not found" });

            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBanner([FromForm] CreateBannerDTO dto, [FromForm] IFormFile image)
        {
            var response = await _bannerService.CreateBannerAsync(dto, image);
            if (response?.Data == null)
                return BadRequest(new { message = response?.Message ?? "Failed to create banner" });

            return CreatedAtAction(nameof(GetBanner), new { id = response.Data.Id }, response.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBanner(int id, [FromForm] UpdateBannerDTO dto, [FromForm] IFormFile image)
        {
            var response = await _bannerService.UpdateBannerAsync(id, dto, image);
            if (response?.Data == null)
                return NotFound(new { message = response?.Message ?? $"Banner with id {id} not found" });

            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var response = await _bannerService.DeleteBannerAsync(id);
            if (response?.Data == null || response.Data == false)
                return NotFound(new { message = response?.Message ?? $"Banner with id {id} not found" });

            return NoContent();
        }
    }
}
