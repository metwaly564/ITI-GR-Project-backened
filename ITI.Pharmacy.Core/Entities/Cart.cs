using System.ComponentModel.DataAnnotations.Schema;

namespace ITI.Pharmacy.Core.Entities
{
    public class Cart
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        
        public virtual User User { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
