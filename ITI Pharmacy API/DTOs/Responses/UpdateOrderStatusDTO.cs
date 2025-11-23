namespace ITI.Pharmacy.API.DTOs
{
    public class UpdateOrderStatusDTO
    {
        public required string Status { get; set; } = string.Empty;
        public string? AdminNotes { get; set; }
    }
}