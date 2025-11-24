using ITI.Pharmacy.API.DTOs;       
using ITI.Pharmacy.API.Services;    
using Microsoft.AspNetCore.Mvc;    
using Microsoft.AspNetCore.Http;   

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> CreateOrder([FromForm] CreateOrderDTO dto)
        {
            var response = await _orderService.CreateOrderAsync(dto, dto.PrescriptionImage);
            if (response?.Data == null)
                return BadRequest(new { message = response?.Message ?? "Failed to create order" });

            return CreatedAtAction(nameof(GetOrder), new { id = response.Data.Id }, response.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDTO>> GetOrder(int id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);
            if (response?.Data == null)
                return NotFound(new { message = response?.Message ?? "Order not found" });

            return Ok(response.Data);
        }
    }
}
