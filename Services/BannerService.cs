using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
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

        public async Task<List<BannerDTO>> GetAllBannersAsync()
        {
            var banners = await _context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<BannerDTO>>(banners);
        }

        public async Task<BannerDTO> GetBannerByIdAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            return _mapper.Map<BannerDTO>(banner);
        }

        public async Task<BannerDTO> CreateBannerAsync(CreateBannerDTO dto, IFormFile image)
        {
            var banner = _mapper.Map<Banner>(dto);

            if (image == null)
                throw new Exception("Banner image is required");

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "banners");
            banner.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });

            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return _mapper.Map<BannerDTO>(banner);
        }

        public async Task<BannerDTO> UpdateBannerAsync(int id, UpdateBannerDTO dto, IFormFile image)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return null;

            if (!string.IsNullOrEmpty(dto.Title)) banner.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description)) banner.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.Link)) banner.Link = dto.Link;
            if (dto.DisplayOrder.HasValue) banner.DisplayOrder = dto.DisplayOrder.Value;
            if (dto.IsActive.HasValue) banner.IsActive = dto.IsActive.Value;

            if (image != null)
            {
                
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, "images", "banners", banner.ImageUrl);
                    FileHelper.DeleteFile(oldImagePath);
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "banners");
                banner.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<BannerDTO>(banner);
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return false;

         
            if (!string.IsNullOrEmpty(banner.ImageUrl))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, "images", "banners", banner.ImageUrl);
                FileHelper.DeleteFile(imagePath);
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}