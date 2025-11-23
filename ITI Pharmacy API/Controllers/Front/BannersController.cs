using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Front
{
    [ApiController]
    [Route("api/[controller]")]
    public class BannersController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannersController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet("active")]
        public async Task<ActionResult<AdvancedApiResponse<List<FrontBannerResponse>>>> GetActiveBanners()
        {
            var result = await _bannerService.GetActiveBannersAsync();
            return Ok(result);
        }

        [HttpGet("active/paginated")]
        public async Task<ActionResult<AdvancedApiResponse<PaginationResponse<FrontBannerResponse>>>> GetActiveBannersPaginated(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _bannerService.GetActiveBannersPaginatedAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}