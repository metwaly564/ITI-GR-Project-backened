namespace ITI.Pharmacy.API.DTOs
{
    public class PharmacyBranchDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreatePharmacyBranchDTO
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class ProductStockDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int Quantity { get; set; }
        public int MinimumStockLevel { get; set; }
        public int MaximumStockLevel { get; set; }
        public DateTime LastRestocked { get; set; }
        public bool IsLowStock => Quantity <= MinimumStockLevel;
        public bool IsOutOfStock => Quantity == 0;
    }

    public class UpdateStockDTO
    {
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public int Quantity { get; set; }
    }

    public class StockTransferDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int FromBranchId { get; set; }
        public string FromBranchName { get; set; }
        public int ToBranchId { get; set; }
        public string ToBranchName { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public DateTime RequestedAt { get; set; }
    }

    public class CreateStockTransferDTO
    {
        public int ProductId { get; set; }
        public int FromBranchId { get; set; }
        public int ToBranchId { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; }
    }
}