public class OrderDTO
{
    public int Id { get; set; }
    public required string OrderNumber { get; set; } = string.Empty;
    public required string Status { get; set; } = string.Empty;
    public required string UserId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string ShippingAddress { get; set; } = string.Empty;
    public required string PhoneNumber { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public string? CityName { get; set; }
    public required string CustomerNotes { get; set; } = string.Empty;
    public required string PrescriptionImageUrl { get; set; } = string.Empty;
    public required string PaymentStatus { get; set; } = string.Empty;
    public required string PaymentMethod { get; set; } = string.Empty;
    public required string BranchName { get; set; } = string.Empty;
    public string? UserName { get; set; }
}