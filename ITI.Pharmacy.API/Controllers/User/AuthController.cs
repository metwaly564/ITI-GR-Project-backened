using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.User
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDTO>> Register(RegisterDTO model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO model)
        {
            try
            {
                var result = await _authService.LoginAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDTO>> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var user = await _authService.GetUserProfileAsync(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UserDTO model)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var result = await _authService.UpdateUserProfileAsync(userId, model);
                return result ? Ok() : BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
