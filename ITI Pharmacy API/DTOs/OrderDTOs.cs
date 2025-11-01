using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DeliveryFee { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string CustomerNotes { get; set; }
        public string PrescriptionImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public string BranchName { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new();
    }

    public class CreateOrderDTO
    {
        [Required]
        public int? BranchId { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public string CustomerNotes { get; set; }
        public string PaymentMethod { get; set; } = "Cash";

        public IFormFile PrescriptionImage { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; } = new();
    }

    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class UpdateOrderStatusDTO
    {
        [Required]
        public string Status { get; set; }
        public string AdminNotes { get; set; }
    }
}