using ITI.Pharmacy.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminCitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public AdminCitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCities()
        {
            var cities = await _cityService.GetAllCitiesAsync();
            return Ok(cities);
        }
    }
}
