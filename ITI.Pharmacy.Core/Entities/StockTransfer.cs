using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.Core.Entities
{
    public class StockTransfer
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }

        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Pending"; 
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string RequestedBy { get; set; }
        public string ApprovedBy { get; set; }

   
        public virtual Product Product { get; set; }
        public virtual PharmacyBranch FromBranch { get; set; }
        public virtual PharmacyBranch ToBranch { get; set; }
    }
}
