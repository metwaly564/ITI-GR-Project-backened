using OfficeOpenXml.Style;
using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.Models
{
    public class PharmacyBranch
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

 
        public virtual ICollection<ProductStock> ProductStocks { get; set; } = new List<ProductStock>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<StockTransfer> OutgoingTransfers { get; set; } = new List<StockTransfer>();
        public virtual ICollection<StockTransfer> IncomingTransfers { get; set; } = new List<StockTransfer>();
    }
}