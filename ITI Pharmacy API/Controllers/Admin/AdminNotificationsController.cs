using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminNotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public AdminNotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.UserId))
                return BadRequest("UserId is required.");

            var notification = await _notificationService.CreateNotificationAsync(dto, dto.UserId);
            return Ok(notification);
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastNotification([FromBody] CreateNotificationDTO dto)
        {
            if (string.IsNullOrEmpty(dto.Message))
                return BadRequest("Message is required.");

            await _notificationService.SendNotificationToAllAsync(
                dto.Title ?? "System Notification", 
                dto.Message, 
                dto.Type ?? "Announcement"
            );

            return Ok("Notification broadcasted successfully.");
        }
    }
}
