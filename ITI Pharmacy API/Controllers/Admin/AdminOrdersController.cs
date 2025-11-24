using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var response = await _orderService.GetAllOrdersAsync();
            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response.Data);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response.Data);
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int id)
        {
            var response = await _orderService.CancelOrderAsync(id, "admin");
            if (!response.Success)
                return NotFound(response.Message ?? "Order not found or cannot be cancelled.");

            return Ok("Order cancelled successfully.");
        }

        [HttpGet("branch/{branchId:int}")]
        public async Task<IActionResult> GetOrdersByBranch(int branchId)
        {
            var response = await _orderService.GetOrdersByBranchAsync(branchId);
            if (!response.Success)
                return NotFound(response.Message);

            return Ok(response.Data);
        }
    }
}
