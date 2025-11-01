using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchesController : ControllerBase
    {
        private readonly IPharmacyBranchService _branchService;

        public BranchesController(IPharmacyBranchService branchService)
        {
            _branchService = branchService;
        }

        // GET: api/branches
        [HttpGet]
        public async Task<ActionResult<List<PharmacyBranchDTO>>> GetBranches()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return Ok(branches);
        }

        // GET: api/branches/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PharmacyBranchDTO>> GetBranch(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null) return NotFound();
            return Ok(branch);
        }

    
        [HttpGet("nearby")]
        public async Task<ActionResult<List<PharmacyBranchDTO>>> GetNearbyBranches(double latitude, double longitude, double radius = 10.0)
        {
            var branches = await _branchService.GetAllBranchesAsync();

            var nearbyBranches = branches.Where(b =>
            {
                var distance = CalculateDistanceHaversine(latitude, longitude, b.Latitude, b.Longitude);
                return distance <= radius;
            }).ToList();

            return Ok(nearbyBranches);
        }


   
        private double CalculateDistanceHaversine(double lat1, double lon1, double lat2, double lon2) ////ان شاء الله لمانوسع المشروع نقدر نستخدم  قاعده البيانات 
        {
            double R = 6371; // Radius of Earth in km
            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            return distance;
        }

        private double DegreesToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }
    }
}
