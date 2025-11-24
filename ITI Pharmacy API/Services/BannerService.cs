using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class BannerService : IBannerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public BannerService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<AdvancedApiResponse<List<BannerDTO>>> GetAllBannersAsync()
        {
            var banners = await _context.Banners.OrderBy(b => b.DisplayOrder).ToListAsync();
            var mapped = _mapper.Map<List<BannerDTO>>(banners);
            return AdvancedApiResponse<List<BannerDTO>>.CreateSuccess(mapped);
        }

        public async Task<AdvancedApiResponse<BannerDTO>> GetBannerByIdAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return AdvancedApiResponse<BannerDTO>.CreateNotFound("Banner");

            var mapped = _mapper.Map<BannerDTO>(banner);
            return AdvancedApiResponse<BannerDTO>.CreateSuccess(mapped);
        }

        public async Task<AdvancedApiResponse<BannerDTO>> CreateBannerAsync(CreateBannerDTO dto, IFormFile image)
        {
            if (image == null)
                return AdvancedApiResponse<BannerDTO>.CreateError("Banner image is required");

            var banner = _mapper.Map<Banner>(dto);

            try
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "banners");
                banner.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });

                _context.Banners.Add(banner);
                await _context.SaveChangesAsync();

                var mapped = _mapper.Map<BannerDTO>(banner);
                return AdvancedApiResponse<BannerDTO>.CreateSuccess(mapped, "Banner created successfully");
            }
            catch (Exception ex)
            {
                return AdvancedApiResponse<BannerDTO>.CreateError("Failed to create banner: " + ex.Message);
            }
        }

        public async Task<AdvancedApiResponse<BannerDTO>> UpdateBannerAsync(int id, UpdateBannerDTO dto, IFormFile image)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return AdvancedApiResponse<BannerDTO>.CreateNotFound("Banner");

            if (!string.IsNullOrEmpty(dto.Title)) banner.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description)) banner.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Link)) banner.Link = dto.Link;
            if (dto.DisplayOrder.HasValue) banner.DisplayOrder = dto.DisplayOrder.Value;
            if (dto.IsActive.HasValue) banner.IsActive = dto.IsActive.Value;

            if (image != null)
            {
                try
                {
                    if (!string.IsNullOrEmpty(banner.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_environment.WebRootPath, "images", "banners", banner.ImageUrl);
                        FileHelper.DeleteFile(oldImagePath);
                    }

                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "banners");
                    banner.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
                }
                catch (Exception ex)
                {
                    return AdvancedApiResponse<BannerDTO>.CreateError("Failed to update banner image: " + ex.Message);
                }
            }

            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<BannerDTO>(banner);
            return AdvancedApiResponse<BannerDTO>.CreateSuccess(mapped, "Banner updated successfully");
        }

        public async Task<AdvancedApiResponse<bool>> DeleteBannerAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return AdvancedApiResponse<bool>.CreateNotFound("Banner");

            if (!string.IsNullOrEmpty(banner.ImageUrl))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, "images", "banners", banner.ImageUrl);
                FileHelper.DeleteFile(imagePath);
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();

            return AdvancedApiResponse<bool>.CreateSuccess(true, "Banner deleted successfully");
        }

        public async Task<AdvancedApiResponse<List<FrontBannerResponse>>> GetActiveBannersAsync()
        {
            var banners = await _context.Banners.Where(b => b.IsActive).OrderBy(b => b.DisplayOrder).ToListAsync();
            var mapped = _mapper.Map<List<FrontBannerResponse>>(banners);
            return AdvancedApiResponse<List<FrontBannerResponse>>.CreateSuccess(mapped);
        }

        public async Task<AdvancedApiResponse<PaginationResponse<FrontBannerResponse>>> GetActiveBannersPaginatedAsync(int page = 1, int pageSize = 10)
        {
            var query = _context.Banners.Where(b => b.IsActive).OrderBy(b => b.DisplayOrder);
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var pagination = new PaginationResponse<FrontBannerResponse>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = _mapper.Map<List<FrontBannerResponse>>(items)
            };

            return AdvancedApiResponse<PaginationResponse<FrontBannerResponse>>.CreateSuccess(pagination);
        }
    }
}
