using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Notifications
{
    public interface INotificationDispatcher
    {
        Task DispatchAsync(string userId, NotificationDTO notification);
    }
}

