using System;
using System.Collections.Generic;

namespace ITI.Pharmacy.API.DTOs
{
    // Analytics Summary DTO
    public class AnalyticsSummaryDTO
    {
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenueGrowth { get; set; }
    }

    // Analytics DTO
    public class AnalyticsDTO
    {
        public AnalyticsSummaryDTO Summary { get; set; } = new AnalyticsSummaryDTO();
        public List<OrderTrendDTO> OrderTrends { get; set; } = new List<OrderTrendDTO>();
    }

    // Dashboard Stats DTO
    public class DashboardStatsDTO
    {
        public int TotalOrders { get; set; }
        public int TodayOrders { get; set; }
        public int PendingOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }
        public int TotalCustomers { get; set; }
    }

    // Order Trend DTO
    public class OrderTrendDTO
    {
        public DateTime Date { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    // Product Sales DTO
    public class ProductSalesDTO
    {
        public required string ProductName { get; set; } = string.Empty;
        public required string ProductImage { get; set; } = string.Empty;
        public int SalesCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    // Stock Alerts DTO
    public class StockAlertsDTO
    {
        public int LowStockAlerts { get; set; }
        public int OutOfStockAlerts { get; set; }
        public int TotalAlerts { get; set; }
    }
}
