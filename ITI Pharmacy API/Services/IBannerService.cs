using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;

public interface IBannerService
{
    Task<List<BannerDTO>> GetAllBannersAsync();
    Task<BannerDTO> GetBannerByIdAsync(int id);
    Task<BannerDTO> CreateBannerAsync(CreateBannerDTO dto, IFormFile? image);
    Task<BannerDTO> UpdateBannerAsync(int id, UpdateBannerDTO dto, IFormFile? image);
    Task<bool> DeleteBannerAsync(int id);

    Task<List<FrontBannerResponse>> GetActiveBannersAsync();
    Task<PaginationResponse<FrontBannerResponse>> GetActiveBannersPaginatedAsync(int page = 1, int pageSize = 10);
    Task<List<BannerDTO>> GetMiddleBannersAsync();
}
