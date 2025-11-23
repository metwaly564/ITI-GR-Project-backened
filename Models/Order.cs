using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI.Pharmacy.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public int? BranchId { get; set; }
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        public string Status { get; set; } = "Pending"; 
        public string PaymentStatus { get; set; } = "Pending"; 
        public string PaymentMethod { get; set; } = "Cash"; 

        public string ShippingAddress { get; set; }
        public string CustomerNotes { get; set; }
        public string AdminNotes { get; set; }

        public string PrescriptionImageUrl { get; set; }

        public int? DeliveryBoyId { get; set; }
        public DateTime? EstimatedDelivery { get; set; }
        public DateTime? DeliveredAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual User User { get; set; }
        public virtual PharmacyBranch Branch { get; set; }
        public virtual DeliveryBoy DeliveryBoy { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}