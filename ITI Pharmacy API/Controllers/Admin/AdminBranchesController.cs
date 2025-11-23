using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminBranchesController : ControllerBase
    {
        private readonly IPharmacyBranchService _branchService;

        public AdminBranchesController(IPharmacyBranchService branchService)
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

        [HttpPost]
        public async Task<ActionResult<PharmacyBranchDTO>> CreateBranch(CreatePharmacyBranchDTO dto)
        {
            var branch = await _branchService.CreateBranchAsync(dto);
            return CreatedAtAction(nameof(GetBranch), new { id = branch.Id }, branch);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PharmacyBranchDTO>> UpdateBranch(int id, CreatePharmacyBranchDTO dto)
        {
            var branch = await _branchService.UpdateBranchAsync(id, dto);
            if (branch == null) return NotFound();
            return Ok(branch);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var result = await _branchService.DeleteBranchAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

 
        [HttpGet("{id}/stock")]
        public async Task<ActionResult<List<ProductStockDTO>>> GetBranchStock(int id)
        {
            var stock = await _branchService.GetBranchStockAsync(id);
            return Ok(stock);
        }

        [HttpPut("stock")]
        public async Task<ActionResult<ProductStockDTO>> UpdateStock(UpdateStockDTO dto)
        {
            var stock = await _branchService.UpdateProductStockAsync(dto);
            return Ok(stock);
        }

    
        [HttpGet("{id}/transfers")]
        public async Task<ActionResult<List<StockTransferDTO>>> GetTransfers(int id)
        {
            var transfers = await _branchService.GetBranchTransfersAsync(id);
            return Ok(transfers);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<PaginationResponse<PharmacyBranchDTO>>> FilterBranches(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] string sortBy = "Name",
            [FromQuery] string sortDirection = "ASC")
        {
            var result = await _branchService.FilterBranchesWithPaginationAsync(page, pageSize, search, sortBy, sortDirection);
            return Ok(result);
        }
    }
}
