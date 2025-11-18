using System.ComponentModel.DataAnnotations.Schema;

namespace ITI.Pharmacy.API.Models
{
    public class ProductStock
    {
        public int ProductId { get; set; }
        public int BranchId { get; set; }

        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; } = 10;
        public int MaximumStockLevel { get; set; } = 100;
        public DateTime LastRestocked { get; set; } = DateTime.UtcNow;

        public virtual Product Product { get; set; }
        public virtual PharmacyBranch Branch { get; set; }
    }
}