using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHomeData(
            int branchId,
            int page = 1,
            int limit = 10,
            string? sort = "newest")
        {
            var result = await _homeService.GetHomeDataAsync(branchId, page, limit, sort);
            return Ok(result);
        }
    }
}
