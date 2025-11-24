using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminInventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminInventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("low-stock")]
        public async Task<ActionResult<List<ProductStockDTO>>> GetLowStock()
        {
            var lowStock = await _context.ProductStocks
                .Include(ps => ps.Product)
                .Include(ps => ps.Branch)
                .Where(ps => ps.Quantity <= ps.MinimumStockLevel)
                .Select(ps => new ProductStockDTO
                {
                    ProductId = ps.ProductId,
                    ProductName = ps.Product.Name,
                    BranchId = ps.BranchId,
                    BranchName = ps.Branch.Name,
                    Quantity = ps.Quantity,
                    MinimumStockLevel = ps.MinimumStockLevel,
                    MaximumStockLevel = ps.MaximumStockLevel,
                    LastRestocked = ps.LastRestocked
                })
                .ToListAsync();

            return Ok(lowStock);
        }

        [HttpGet("stock-alerts")]
        public async Task<ActionResult<object>> GetStockAlerts()
        {
            var totalProducts = await _context.Products.CountAsync();
            var outOfStock = await _context.ProductStocks.CountAsync(ps => ps.Quantity == 0);
            var lowStock = await _context.ProductStocks.CountAsync(ps => ps.Quantity > 0 && ps.Quantity <= ps.MinimumStockLevel);

            return new
            {
                TotalProducts = totalProducts,
                OutOfStock = outOfStock,
                LowStock = lowStock,
                HealthyStock = totalProducts - outOfStock - lowStock
            };
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<StockTransfer>> CreateStockTransfer(CreateStockTransferDTO dto)
        {
            // Check if source branch has enough stock
            var sourceStock = await _context.ProductStocks
                .FirstOrDefaultAsync(ps => ps.ProductId == dto.ProductId && ps.BranchId == dto.FromBranchId);

            if (sourceStock == null || sourceStock.Quantity < dto.Quantity)
            {
                return BadRequest("Insufficient stock in source branch");
            }

            var transfer = new StockTransfer
            {
                ProductId = dto.ProductId,
                FromBranchId = dto.FromBranchId,
                ToBranchId = dto.ToBranchId,
                Quantity = dto.Quantity,
                Reason = dto.Reason,
                Status = "Pending",
                RequestedBy = User.Identity.Name,
                RequestedAt = DateTime.UtcNow
            };

            _context.StockTransfers.Add(transfer);
            await _context.SaveChangesAsync();

            return Ok(transfer);
        }

        [HttpPost("transfer/{id}/approve")]
        public async Task<IActionResult> ApproveTransfer(int id)
        {
            var transfer = await _context.StockTransfers
                .Include(st => st.Product)
                .FirstOrDefaultAsync(st => st.Id == id);

            if (transfer == null) return NotFound();

            // Check stock again before approval
            var sourceStock = await _context.ProductStocks
                .FirstOrDefaultAsync(ps => ps.ProductId == transfer.ProductId && ps.BranchId == transfer.FromBranchId);

            if (sourceStock == null || sourceStock.Quantity < transfer.Quantity)
            {
                transfer.Status = "Rejected";
                await _context.SaveChangesAsync();
                return BadRequest("Insufficient stock for transfer");
            }

            transfer.Status = "Approved";
            transfer.ApprovedBy = User.Identity.Name;
            transfer.ApprovedAt = DateTime.UtcNow;

            // Deduct from source branch
            sourceStock.Quantity -= transfer.Quantity;

            // Add to destination branch
            var destStock = await _context.ProductStocks
                .FirstOrDefaultAsync(ps => ps.ProductId == transfer.ProductId && ps.BranchId == transfer.ToBranchId);

            if (destStock == null)
            {
                destStock = new ProductStock
                {
                    ProductId = transfer.ProductId,
                    BranchId = transfer.ToBranchId,
                    Quantity = transfer.Quantity,
                    LastRestocked = DateTime.UtcNow
                };
                _context.ProductStocks.Add(destStock);
            }
            else
            {
                destStock.Quantity += transfer.Quantity;
                destStock.LastRestocked = DateTime.UtcNow;
            }

            transfer.Status = "Completed";
            transfer.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(transfer);
        }
    }
}
