using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId);
        Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto, string userId); // ?????? ????? ??????? ????
        Task<bool> MarkAsReadAsync(int notificationId, string userId);
        Task<bool> MarkAllAsReadAsync(string userId);

   
        Task<List<string>> GetAllUserIdsAsync();
    }
}
