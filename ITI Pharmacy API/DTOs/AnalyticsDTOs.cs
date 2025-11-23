using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    public class AnalyticsDTO
    {
        [JsonPropertyName("summary")]
        public AnalyticsSummaryDTO Summary { get; set; } = new AnalyticsSummaryDTO();

        [JsonPropertyName("orderTrends")]
        public List<OrderTrendDTO> OrderTrends { get; set; } = new List<OrderTrendDTO>();

        [JsonPropertyName("topSellingProducts")]
        public List<ProductSalesDTO> TopSellingProducts { get; set; } = new List<ProductSalesDTO>();
    }

    public class AnalyticsSummaryDTO
    {
        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        [JsonPropertyName("totalOrders")]
        public int TotalOrders { get; set; }

        [JsonPropertyName("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("totalCustomers")]
        public int TotalCustomers { get; set; }

        [JsonPropertyName("revenueGrowth")]
        public decimal RevenueGrowth { get; set; }
    }

    public class OrderTrendDTO
    {
        [JsonPropertyName("period")]
        public string Period { get; set; } = string.Empty;

        [JsonPropertyName("revenue")]
        public decimal Revenue { get; set; }

        [JsonPropertyName("ordersCount")]
        public int OrdersCount { get; set; }
    }

    public class ProductSalesDTO
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        [JsonPropertyName("quantitySold")]
        public int QuantitySold { get; set; }
    }

    public class DashboardStatsDTO
    {
        [JsonPropertyName("totalOrders")]
        public int TotalOrders { get; set; }

        [JsonPropertyName("todayOrders")]
        public int TodayOrders { get; set; }

        [JsonPropertyName("pendingOrders")]
        public int PendingOrders { get; set; }

        [JsonPropertyName("totalRevenue")]
        public decimal TotalRevenue { get; set; }

        [JsonPropertyName("lowStockProducts")]
        public int LowStockProducts { get; set; }

        [JsonPropertyName("outOfStockProducts")]
        public int OutOfStockProducts { get; set; }

        [JsonPropertyName("totalCustomers")]
        public int TotalCustomers { get; set; }
    }
}