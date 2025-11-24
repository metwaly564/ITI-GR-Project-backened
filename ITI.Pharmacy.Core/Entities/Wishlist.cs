namespace ITI.Pharmacy.Core.Entities
{
    public class Wishlist
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

 
        public virtual User User { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
