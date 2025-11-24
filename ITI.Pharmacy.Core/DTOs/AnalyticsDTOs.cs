namespace ITI.Pharmacy.Core.DTOs
{
    public class AnalyticsDTO
    {
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<OrderTrendDTO> OrderTrends { get; set; } = new();
        public List<ProductSalesDTO> TopSellingProducts { get; set; } = new();
    }

    public class OrderTrendDTO
    {
        public string Period { get; set; }
        public int OrdersCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ProductSalesDTO
    {
        public int ProductId { get; set; }
        public required string ProductName { get; set; }
        public int QuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
