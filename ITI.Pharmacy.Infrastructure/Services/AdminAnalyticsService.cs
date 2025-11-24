using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
{
    public class AdminAnalyticsService : IAdminAnalyticsService
    {
        private readonly ApplicationDbContext _context;

        public AdminAnalyticsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AnalyticsDTO> GetAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            startDate ??= DateTime.UtcNow.AddDays(-30);
            endDate ??= DateTime.UtcNow;

            var totalOrders = await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .CountAsync();

            var totalProducts = await _context.Products.CountAsync(p => p.IsActive);
            var totalUsers = await _context.Users.CountAsync(u => u.IsActive);
            var totalRevenue = await _context.Orders
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate && o.PaymentStatus == "Paid")
                .SumAsync(o => o.TotalAmount + o.DeliveryFee);

            // Order trends (last 7 days)
            var orderTrends = new List<OrderTrendDTO>();
            for (int i = 6; i >= 0; i--)
            {
                var date = DateTime.UtcNow.AddDays(-i).Date;
                var nextDate = date.AddDays(1);

                var dayOrders = await _context.Orders
                    .Where(o => o.CreatedAt >= date && o.CreatedAt < nextDate)
                    .ToListAsync();

                orderTrends.Add(new OrderTrendDTO
                {
                    Period = date.ToString("MMM dd"),
                    OrdersCount = dayOrders.Count,
                    Revenue = dayOrders.Where(o => o.PaymentStatus == "Paid").Sum(o => o.TotalAmount + o.DeliveryFee)
                });
            }

            // Top selling products
            var topSellingProducts = await _context.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.Order.CreatedAt >= startDate && oi.Order.CreatedAt <= endDate)
                .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
                .Select(g => new ProductSalesDTO
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.Name,
                    QuantitySold = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.TotalPrice)
                })
                .OrderByDescending(p => p.QuantitySold)
                .Take(10)
                .ToListAsync();

            return new AnalyticsDTO
            {
                TotalOrders = totalOrders,
                TotalProducts = totalProducts,
                TotalUsers = totalUsers,
                TotalRevenue = totalRevenue,
                OrderTrends = orderTrends,
                TopSellingProducts = topSellingProducts
            };
        }

        public async Task<object> GetDashboardStatsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var totalOrders = await _context.Orders.CountAsync();
            var todayOrders = await _context.Orders.CountAsync(o => o.CreatedAt >= today && o.CreatedAt < tomorrow);
            var pendingOrders = await _context.Orders.CountAsync(o => o.Status == "Pending");
            var totalRevenue = await _context.Orders
                .Where(o => o.PaymentStatus == "Paid")
                .SumAsync(o => o.TotalAmount + o.DeliveryFee);

            var lowStockProducts = await _context.ProductStocks
                .CountAsync(ps => ps.Quantity <= ps.MinimumStockLevel);

            var totalCustomers = await _context.Users.CountAsync(u => u.IsActive);

            return new
            {
                TotalOrders = totalOrders,
                TodayOrders = todayOrders,
                PendingOrders = pendingOrders,
                TotalRevenue = totalRevenue,
                LowStockProducts = lowStockProducts,
                TotalCustomers = totalCustomers
            };
        }
    }
}
