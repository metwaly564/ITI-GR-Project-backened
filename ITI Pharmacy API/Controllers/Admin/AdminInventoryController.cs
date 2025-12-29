using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminInventoryController : ControllerBase
    {
        private readonly IPharmacyBranchService _branchService;
        private readonly INotificationService _notificationService;

        public AdminInventoryController(IPharmacyBranchService branchService, INotificationService notificationService)
        {
            _branchService = branchService;
            _notificationService = notificationService;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> InitiateTransfer([FromBody] CreateStockTransferDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "Admin";
            var result = await _branchService.InitiateTransferAsync(dto, userId);
            
            if (result == null)
                return BadRequest("Invalid transfer request or insufficient stock.");

            return Ok(result);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            var allLowStock = new List<ProductStockDTO>();

            foreach (var branch in branches)
            {
                var stock = await _branchService.GetBranchStockAsync(branch.Id);
                allLowStock.AddRange(stock.Where(s => s.IsLowStock));
            }

            return Ok(allLowStock);
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpiredProducts()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            var allExpired = new List<ProductStockDTO>();

            foreach (var branch in branches)
            {
                var stock = await _branchService.GetBranchStockAsync(branch.Id);
                // allExpired.AddRange(stock.Where(s => s.IsExpired));
            }

            return Ok(allExpired);
        }

        [HttpPost("trigger-alerts")]
        public async Task<IActionResult> TriggerStockAlerts()
        {
            var branches = await _branchService.GetAllBranchesAsync();
            int alertCount = 0;

            foreach (var branch in branches)
            {
                var stock = await _branchService.GetBranchStockAsync(branch.Id);
                
                // Low Stock Alerts
                var lowStock = stock.Where(s => s.IsLowStock).ToList();
                foreach (var item in lowStock)
                {
                    await _notificationService.CreateNotificationAsync(new CreateNotificationDTO {
                        Message = $"Low stock alert: {item.ProductName} in {item.BranchName} (Quantity: {item.Quantity})",
                        Type = "LowStock"
                    }, "Admin"); // Or notify all admins
                    alertCount++;
                }

                // Expiry Alerts
                /*
                var expired = stock.Where(s => s.IsExpired).ToList();
                foreach (var item in expired)
                {
                    await _notificationService.CreateNotificationAsync(new CreateNotificationDTO {
                        Message = $"Expired product alert: {item.ProductName} in {item.BranchName} expired on {item.ExpiryDate?.ToShortDateString()}",
                        Type = "Expired"
                    }, "Admin");
                    alertCount++;
                }
                */
            }

            return Ok(new { alertsSent = alertCount });
        }
    }
}
