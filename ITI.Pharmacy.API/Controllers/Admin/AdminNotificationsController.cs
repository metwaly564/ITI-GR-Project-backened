using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Services;
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

            var allUsers = await _notificationService.GetAllUserIdsAsync();

            var tasks = allUsers.Select(userId =>
                _notificationService.CreateNotificationAsync(dto, userId));

            await Task.WhenAll(tasks);

            return Ok($"Notification sent to {allUsers.Count} users successfully.");
        }
    }
}
