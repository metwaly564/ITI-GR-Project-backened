using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Hubs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.SignalR;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId)
    {
        // تنفيذ مؤقت - إرجاع قائمة فارغة
        return await Task.FromResult(new List<NotificationDTO>());
    }

    public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto, string userId)
    {
        // التحقق من null للمعاملات
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        if (string.IsNullOrEmpty(userId))
            throw new ArgumentNullException(nameof(userId));

        var notification = new NotificationDTO
        {
            Id = 1,
            Title = dto.Title ?? string.Empty,
            Message = dto.Message ?? string.Empty,
            Type = dto.Type ?? "Info",
            UserId = userId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

     
        await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", notification);

        return notification;
    }

    public async Task<bool> MarkAsReadAsync(int notificationId, string userId)
    {
     
        return await Task.FromResult(true);
    }

    public async Task<bool> MarkAllAsReadAsync(string userId)
    {
  
        return await Task.FromResult(true);
    }

    public async Task<List<string>> GetAllUserIdsAsync()
    {
    
        return await Task.FromResult(new List<string> { "user1", "user2", "user3" });
    }
}