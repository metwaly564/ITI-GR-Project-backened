using System.Text.Json.Serialization;

namespace ITI.Pharmacy.API.DTOs
{
    public class ProductStockDTO
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("branchId")]
        public int BranchId { get; set; }

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("minimumStockLevel")]
        public int MinimumStockLevel { get; set; }

        [JsonPropertyName("maximumStockLevel")]
        public int MaximumStockLevel { get; set; }

        [JsonPropertyName("lastRestocked")]
        public DateTime? LastRestocked { get; set; }

        [JsonPropertyName("isLowStock")]
        public bool IsLowStock => Quantity > 0 && Quantity <= MinimumStockLevel;

        [JsonPropertyName("isOutOfStock")]
        public bool IsOutOfStock => Quantity == 0;
    }

    public class UpdateStockDTO
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("branchId")]
        public int BranchId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
    }

    public class StockTransferDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("fromBranchId")]
        public int FromBranchId { get; set; }

        [JsonPropertyName("fromBranchName")]
        public string FromBranchName { get; set; } = string.Empty;

        [JsonPropertyName("toBranchId")]
        public int ToBranchId { get; set; }

        [JsonPropertyName("toBranchName")]
        public string ToBranchName { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty; // "Pending", "Approved", "Completed", "Cancelled"

        [JsonPropertyName("requestedAt")]
        public DateTime RequestedAt { get; set; }

        [JsonPropertyName("completedAt")]
        public DateTime? CompletedAt { get; set; }
    }

    public class CreateStockTransferDTO
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("fromBranchId")]
        public int FromBranchId { get; set; }

        [JsonPropertyName("toBranchId")]
        public int ToBranchId { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;
    }

    public class UpdateStockTransferDTO
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty; // "Approved", "Completed", "Cancelled"

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }
    }

    public class StockAlertDTO
    {
        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("branchId")]
        public int BranchId { get; set; }

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("currentQuantity")]
        public int CurrentQuantity { get; set; }

        [JsonPropertyName("minimumStockLevel")]
        public int MinimumStockLevel { get; set; }

        [JsonPropertyName("alertType")]
        public string AlertType { get; set; } = string.Empty; // "LowStock", "OutOfStock", "OverStock"

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("alertDate")]
        public DateTime AlertDate { get; set; } = DateTime.UtcNow;
    }

    public class BranchStockDTO
    {
        [JsonPropertyName("branchId")]
        public int BranchId { get; set; }

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("products")]
        public List<ProductStockDTO> Products { get; set; } = new List<ProductStockDTO>();

        [JsonPropertyName("totalProducts")]
        public int TotalProducts => Products.Count;

        [JsonPropertyName("lowStockCount")]
        public int LowStockCount => Products.Count(p => p.Quantity <= p.MinimumStockLevel && p.Quantity > 0);

        [JsonPropertyName("outOfStockCount")]
        public int OutOfStockCount => Products.Count(p => p.Quantity == 0);

        [JsonPropertyName("overStockCount")]
        public int OverStockCount => Products.Count(p => p.Quantity > p.MaximumStockLevel);
    }

    public class StockHistoryDTO
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("productId")]
        public int ProductId { get; set; }

        [JsonPropertyName("productName")]
        public string ProductName { get; set; } = string.Empty;

        [JsonPropertyName("branchId")]
        public int BranchId { get; set; }

        [JsonPropertyName("branchName")]
        public string BranchName { get; set; } = string.Empty;

        [JsonPropertyName("changeType")]
        public string ChangeType { get; set; } = string.Empty; // "Restock", "Sale", "Transfer", "Adjustment"

        [JsonPropertyName("previousQuantity")]
        public int PreviousQuantity { get; set; }

        [JsonPropertyName("newQuantity")]
        public int NewQuantity { get; set; }

        [JsonPropertyName("changeAmount")]
        public int ChangeAmount => NewQuantity - PreviousQuantity;

        [JsonPropertyName("reason")]
        public string Reason { get; set; } = string.Empty;

        [JsonPropertyName("changedBy")]
        public string ChangedBy { get; set; } = string.Empty;

        [JsonPropertyName("changedAt")]
        public DateTime ChangedAt { get; set; }
    }

    public class StockSummaryDTO
    {
        [JsonPropertyName("totalProducts")]
        public int TotalProducts { get; set; }

        [JsonPropertyName("totalBranches")]
        public int TotalBranches { get; set; }

        [JsonPropertyName("totalStockValue")]
        public decimal TotalStockValue { get; set; }

        [JsonPropertyName("lowStockAlerts")]
        public int LowStockAlerts { get; set; }

        [JsonPropertyName("outOfStockAlerts")]
        public int OutOfStockAlerts { get; set; }

        [JsonPropertyName("pendingTransfers")]
        public int PendingTransfers { get; set; }
    }
}