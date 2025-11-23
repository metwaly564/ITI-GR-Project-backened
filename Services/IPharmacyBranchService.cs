using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IPharmacyBranchService
    {
        Task<List<PharmacyBranchDTO>> GetAllBranchesAsync();
        Task<PharmacyBranchDTO> GetBranchByIdAsync(int id);
        Task<PharmacyBranchDTO> CreateBranchAsync(CreatePharmacyBranchDTO dto);
        Task<PharmacyBranchDTO> UpdateBranchAsync(int id, CreatePharmacyBranchDTO dto);
        Task<bool> DeleteBranchAsync(int id);
        Task<List<ProductStockDTO>> GetBranchStockAsync(int branchId);
        Task<ProductStockDTO> UpdateProductStockAsync(UpdateStockDTO dto);
        Task<List<StockTransferDTO>> GetBranchTransfersAsync(int branchId);

 
        Task<PaginationResponse<PharmacyBranchDTO>> FilterBranchesWithPaginationAsync(
            int page,
            int pageSize,
            string? search = null,
            string sortBy = "Name",
            string sortDirection = "ASC");
    }
}
