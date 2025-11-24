using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<CartDTO>> GetCart()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CartDTO>> AddToCart(AddToCartDTO dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var cart = await _cartService.AddToCartAsync(userId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("items/{itemId}")]
        public async Task<ActionResult<CartDTO>> UpdateCartItem(int itemId, UpdateCartItemDTO dto)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var cart = await _cartService.UpdateCartItemAsync(userId, itemId, dto);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _cartService.RemoveFromCartAsync(userId, itemId);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _cartService.ClearCartAsync(userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
