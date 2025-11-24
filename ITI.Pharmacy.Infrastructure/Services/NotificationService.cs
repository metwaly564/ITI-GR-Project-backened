using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Notifications;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;
        private readonly INotificationDispatcher _dispatcher;

        public NotificationService(ApplicationDbContext context, INotificationDispatcher dispatcher)
        {
            _context = context;
            _dispatcher = dispatcher;
        }

        public async Task<List<NotificationDTO>> GetUserNotificationsAsync(string userId)
        {
            var notifications = await _context.UserNotifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications.Select(MapToDto).ToList();
        }

        public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto, string userId)
        {
            var entity = new UserNotification
            {
                UserId = userId,
                Title = dto.Title,
                Message = dto.Message,
                Type = dto.Type,
                IsRead = false
            };

            _context.UserNotifications.Add(entity);
            await _context.SaveChangesAsync();

            var notificationDto = MapToDto(entity);
            await _dispatcher.DispatchAsync(userId, notificationDto);
            return notificationDto;
        }

        public async Task<bool> MarkAsReadAsync(int notificationId, string userId)
        {
            var notification = await _context.UserNotifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

            if (notification == null) return false;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkAllAsReadAsync(string userId)
        {
            var notifications = await _context.UserNotifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync();

            if (!notifications.Any()) return false;

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetAllUserIdsAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .Select(u => u.Id)
                .ToListAsync();
        }

        private static NotificationDTO MapToDto(UserNotification notification)
        {
            return new NotificationDTO
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead,
                Type = notification.Type,
                CreatedAt = notification.CreatedAt
            };
        }
    }
}
