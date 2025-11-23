public class ProductSalesDTO
{
    public required string ProductName { get; set; } = string.Empty;
    public required string ProductImage { get; set; } = string.Empty;
    public int SalesCount { get; set; }
    public decimal TotalRevenue { get; set; }
}