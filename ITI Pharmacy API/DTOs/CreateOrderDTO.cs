using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.DTOs
{
    public class CreateOrderDTO
    {
        [Required(ErrorMessage = "Shipping address is required")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "Shipping address must be between 10 and 1000 characters")]
        public required string ShippingAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Phone number must be between 8 and 20 characters")]
        public required string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid city")]
        public required int CityId { get; set; }

        [StringLength(500, ErrorMessage = "Customer notes cannot exceed 500 characters")]
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