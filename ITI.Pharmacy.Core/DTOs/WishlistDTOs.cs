namespace ITI.Pharmacy.Core.DTOs
{
    public class WishlistDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public List<WishlistItemDTO> Items { get; set; } = new();
    }

    public class WishlistItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
