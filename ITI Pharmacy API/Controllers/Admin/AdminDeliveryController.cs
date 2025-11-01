using ITI.Pharmacy.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminDeliveryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminDeliveryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("boys")]
        public async Task<IActionResult> GetDeliveryBoys()
        {
            var deliveryBoys = await _context.DeliveryBoys
                .Where(d => d.IsActive)
                .ToListAsync();

            return Ok(deliveryBoys);
        }

        [HttpPost("boys")]
        public async Task<IActionResult> CreateDeliveryBoy(DeliveryBoy deliveryBoy)
        {
            _context.DeliveryBoys.Add(deliveryBoy);
            await _context.SaveChangesAsync();
            return Ok(deliveryBoy);
        }

        [HttpPut("assign-order/{orderId}")]
        public async Task<IActionResult> AssignOrderToDeliveryBoy(int orderId, [FromQuery] int deliveryBoyId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            var deliveryBoy = await _context.DeliveryBoys.FindAsync(deliveryBoyId);
            if (deliveryBoy == null) return NotFound();

            order.DeliveryBoyId = deliveryBoyId;
            order.Status = "Shipped";
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}