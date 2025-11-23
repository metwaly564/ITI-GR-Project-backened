using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models.Responses;

namespace ITI.Pharmacy.API.Services
{
    public interface IHomeService
    {
        Task<AdvancedApiResponse<PaginationResponse<ProductDTO>>> GetHomeDataAsync(
            int branchId, int page, int limit, string? sort);
    }
}