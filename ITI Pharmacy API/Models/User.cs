using Microsoft.AspNetCore.Identity;
using OfficeOpenXml.Style;
using System.ComponentModel.DataAnnotations;

namespace ITI.Pharmacy.API.Models
{
    public class User : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual Cart Cart { get; set; }
        public virtual Wishlist Wishlist { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual ICollection<UserNotification> Notifications { get; set; } = new List<UserNotification>();
    }
}