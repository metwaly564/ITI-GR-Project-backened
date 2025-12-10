namespace ITI.Pharmacy.API.DTOs
{
    public class CreateOrderDTO
    {
        public required string ShippingAddress { get; set; } = string.Empty;
        public required string CustomerNotes { get; set; } = string.Empty;
        public required string PrescriptionImageUrl { get; set; } = string.Empty;
        public required string PaymentMethod { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new();
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}