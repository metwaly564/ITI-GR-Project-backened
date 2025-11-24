using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
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
        public async Task<ActionResult<List<OrderDTO>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Order not found.");

            return Ok(order);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusDTO dto)
        {
            var result = await _orderService.UpdateOrderStatusAsync(id, dto);
            if (!result)
                return NotFound("Order not found.");

            return Ok("Order status updated successfully.");
        }

        [HttpGet("branch/{branchId:int}")]
        public async Task<ActionResult<List<OrderDTO>>> GetOrdersByBranch(int branchId)
        {
            var orders = await _orderService.GetOrdersByBranchAsync(branchId);
            return Ok(orders);
        }
    }
}
