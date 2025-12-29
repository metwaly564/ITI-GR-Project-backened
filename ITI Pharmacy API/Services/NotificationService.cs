using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Hubs;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.SignalR;

public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }


    public async Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId)
    {

        return new List<NotificationDTO>(); // مؤقت
    }


    public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto, string userId)
    {
        var notification = new NotificationDTO
        {
            Id = new Random().Next(1, 10000),
            Message = dto.Message,
            Title = dto.Title ?? "Notification",
            Type = dto.Type ?? "General",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };


        await _hub.Clients.Group(userId).SendAsync("ReceiveNotification", notification);


        return notification;
    }

    public async Task SendNotificationToAllAsync(string title, string message, string type)
    {
        var notification = new NotificationDTO
        {
            Id = new Random().Next(1, 10000),
            Message = message,
            Title = title,
            Type = type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _hub.Clients.All.SendAsync("ReceiveNotification", notification);
    }


    public async Task<bool> MarkAsReadAsync(int notificationId, string userId)
    {
  
        return true; // مؤقت
    }


    public async Task<bool> MarkAllAsReadAsync(string userId)
    {
        return true; 
    }

    public async Task<List<string>> GetAllUserIdsAsync()
    {
      
        return new List<string> { "user1", "user2", "user3" }; 
    }
}
