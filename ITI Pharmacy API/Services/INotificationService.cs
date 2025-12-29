using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId);
        Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto, string userId); // لإرسال إشعار لمستخدم محدد
        Task SendNotificationToAllAsync(string title, string message, string type);
        Task<bool> MarkAsReadAsync(int notificationId, string userId);
        Task<bool> MarkAllAsReadAsync(string userId);

   
        Task<List<string>> GetAllUserIdsAsync();
    }
}
