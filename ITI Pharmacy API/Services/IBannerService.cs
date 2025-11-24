using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models.Responses;

public interface IBannerService
{
    Task<AdvancedApiResponse<List<BannerDTO>>> GetAllBannersAsync();
    Task<AdvancedApiResponse<BannerDTO>> GetBannerByIdAsync(int id);
    Task<AdvancedApiResponse<BannerDTO>> CreateBannerAsync(CreateBannerDTO dto, IFormFile image);
    Task<AdvancedApiResponse<BannerDTO>> UpdateBannerAsync(int id, UpdateBannerDTO dto, IFormFile image);
    Task<AdvancedApiResponse<bool>> DeleteBannerAsync(int id);

    Task<AdvancedApiResponse<List<FrontBannerResponse>>> GetActiveBannersAsync();
    Task<AdvancedApiResponse<PaginationResponse<FrontBannerResponse>>> GetActiveBannersPaginatedAsync(int page = 1, int pageSize = 10);
}
