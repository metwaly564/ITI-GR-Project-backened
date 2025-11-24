using ITI.Pharmacy.API.Hubs;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Interfaces.Notifications;
using Microsoft.AspNetCore.SignalR;

namespace ITI.Pharmacy.API.Services
{
    public class SignalRNotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationDispatcher(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task DispatchAsync(string userId, NotificationDTO notification)
        {
            if (string.IsNullOrWhiteSpace(userId)) return;
            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}

