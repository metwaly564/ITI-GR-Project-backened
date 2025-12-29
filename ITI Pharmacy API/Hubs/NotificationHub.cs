using Microsoft.AspNetCore.SignalR;

namespace ITI.Pharmacy.API.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId); // كل مستخدم في مجموعة خاصة
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToAll(string title, string message, string type)
        {
            await Clients.All.SendAsync("ReceiveNotification", new { title, message, type, createdAt = DateTime.UtcNow });
        }

        public async Task SendMessageToUser(string userId, string title, string message, string type)
        {
            await Clients.Group(userId).SendAsync("ReceiveNotification", new { title, message, type, createdAt = DateTime.UtcNow });
        }
    }
}
