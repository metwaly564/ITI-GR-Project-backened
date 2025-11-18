using ITI.Pharmacy.API.DTOs;
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
        public async Task<ActionResult<List<BannerDTO>>> GetBanners()
        {
            var banners = await _bannerService.GetAllBannersAsync();
            return Ok(banners);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BannerDTO>> GetBanner(int id)
        {
            var banner = await _bannerService.GetBannerByIdAsync(id);
            if (banner == null) return NotFound();
            return Ok(banner);
        }

        [HttpPost]
        public async Task<ActionResult<BannerDTO>> CreateBanner([FromForm] CreateBannerDTO dto, IFormFile image)
        {
            try
            {
                var banner = await _bannerService.CreateBannerAsync(dto, image);
                return CreatedAtAction(nameof(GetBanner), new { id = banner.Id }, banner);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BannerDTO>> UpdateBanner(int id, [FromForm] UpdateBannerDTO dto, IFormFile image)
        {
            try
            {
                var banner = await _bannerService.UpdateBannerAsync(id, dto, image);
                if (banner == null) return NotFound();
                return Ok(banner);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBanner(int id)
        {
            var result = await _bannerService.DeleteBannerAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}