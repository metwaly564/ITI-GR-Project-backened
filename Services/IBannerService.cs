using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IBannerService
    {
        Task<List<BannerDTO>> GetAllBannersAsync();
        Task<BannerDTO> GetBannerByIdAsync(int id);
        Task<BannerDTO> CreateBannerAsync(CreateBannerDTO dto, IFormFile image);
        Task<BannerDTO> UpdateBannerAsync(int id, UpdateBannerDTO dto, IFormFile image);
        Task<bool> DeleteBannerAsync(int id);
    }
}