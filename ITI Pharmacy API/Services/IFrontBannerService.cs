using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;

namespace ITI.Pharmacy.API.Services
{
    public interface IFrontBannerService
    {
        Task<PaginationResponse<FrontBannerResponse>> GetActiveBannersAsync(int page, int pageSize);
    }
}
