namespace ITI.Pharmacy.Core.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CartItemDTO> CartItems { get; set; } = new();
    }

    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDTO
    {
        public int Quantity { get; set; }
    }
}
