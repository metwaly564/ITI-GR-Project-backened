using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
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
  
            return new AnalyticsDTO
            {
                Summary = new AnalyticsSummaryDTO
                {
                    TotalOrders = 0,
                    TotalProducts = 0,
                    TotalCustomers = 0,
                    TotalRevenue = 0,
                    RevenueGrowth = 0
                },
               
            };
        }

        public async Task<DashboardStatsDTO> GetDashboardStatsAsync()
        {
            return new DashboardStatsDTO
            {
                TotalOrders = 0,
                TodayOrders = 0,
                PendingOrders = 0,
                TotalRevenue = 0,
                LowStockProducts = 0,
                OutOfStockProducts = 0,
                TotalCustomers = 0
            };
        }

        public async Task<List<OrderTrendDTO>> GetRevenueTrendAsync(int days = 30)
        {
            return new List<OrderTrendDTO>();
        }

        public async Task<List<ProductSalesDTO>> GetTopSellingProductsAsync(int top = 10, DateTime? startDate = null, DateTime? endDate = null)
        {
            return new List<ProductSalesDTO>();
        }
    }
}