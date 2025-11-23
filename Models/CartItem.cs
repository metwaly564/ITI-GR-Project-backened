using System.ComponentModel.DataAnnotations.Schema;

namespace ITI.Pharmacy.API.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

  
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}