using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder(CreateOrderDTO dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token. Please log in again." });
                }
                
                var order = await _orderService.CreateOrderAsync(userId, dto);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                // Return specific validation errors
                return BadRequest(new { message = ex.Message });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                // Handle database constraint violations
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                Console.WriteLine($"Database error creating order: {innerMessage}");
                Console.WriteLine($"Stack trace: {dbEx.StackTrace}");
                
                // Check for common constraint violations
                if (innerMessage.Contains("FOREIGN KEY") || innerMessage.Contains("foreign key"))
                {
                    return BadRequest(new { message = "Invalid reference in order data. Please check that the city, branch, and products are valid." });
                }
                if (innerMessage.Contains("PRIMARY KEY") || innerMessage.Contains("primary key"))
                {
                    return BadRequest(new { message = "Duplicate order detected. Please try again." });
                }
                
                return BadRequest(new { message = $"Database error: {innerMessage}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating order: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> GetUserOrders()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var orders = await _orderService.GetUserOrdersAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var order = await _orderService.GetOrderByIdAsync(id, userId);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _orderService.CancelOrderAsync(id, userId);
            if (!result) return BadRequest("Unable to cancel order");
            return Ok();
        }
    }
}