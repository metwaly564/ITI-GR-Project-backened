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

        [HttpGet]
        public async Task<ActionResult<List<PharmacyBranchDTO>>> GetAllBranches()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            return Ok(branches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PharmacyBranchDTO>> GetBranch(int id)
        {
            var branch = await _branchService.GetBranchByIdAsync(id);
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        [HttpGet("{id}/stock")]
        public async Task<ActionResult<List<ProductStockDTO>>> GetBranchStock(int id)
        {
            var stock = await _branchService.GetBranchStockAsync(id);
            return Ok(stock);
        }
    }
}
