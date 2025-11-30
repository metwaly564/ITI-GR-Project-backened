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
        public async Task<IActionResult> GetHomeData(int branchId = 0)
        {
            try
            {
                var result = await _homeService.GetHomeDataAsync(branchId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving home data", error = ex.Message });
            }
        }
    }
}
