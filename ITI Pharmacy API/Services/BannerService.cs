using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IO;

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
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<BannerDTO>>(banners);
        }

        public async Task<BannerDTO> GetBannerByIdAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            return _mapper.Map<BannerDTO>(banner);
        }

        public async Task<BannerDTO> CreateBannerAsync(CreateBannerDTO dto, IFormFile? image)
        {
            // Convert int Type to BannerType enum before mapping
            var banner = new Banner
            {
                // Don't set Id - it's an identity column, EF will generate it
                Title = dto.Title,
                Description = dto.Description,
                Link = dto.Link,
                DisplayOrder = dto.DisplayOrder,
                Type = (BannerType)dto.Type, // Convert int to enum
                ImageUrl = dto.ImageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            if (image == null)
                throw new Exception("Banner image is required");

            // Get WebRootPath or fallback to ContentRootPath/wwwroot
            var webRootPath = _environment.WebRootPath;
            if (string.IsNullOrEmpty(webRootPath))
            {
                // Fallback: use ContentRootPath and create wwwroot if needed
                webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                if (!Directory.Exists(webRootPath))
                {
                    Directory.CreateDirectory(webRootPath);
                }
            }

            var uploadsFolder = Path.Combine(webRootPath, "images", "banners");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var fileName = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
            // Store the relative path for serving static files
            banner.ImageUrl = $"images/banners/{fileName}";

            // Add the banner - EF will handle the identity column automatically
            _context.Banners.Add(banner);
            await _context.SaveChangesAsync();

            return _mapper.Map<BannerDTO>(banner);
        }

        public async Task<BannerDTO> UpdateBannerAsync(int id, UpdateBannerDTO dto, IFormFile? image)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return null;

            // Update fields if provided (check for null, not empty string, to allow clearing fields)
            if (dto.Title != null) banner.Title = dto.Title;
            if (dto.Description != null) banner.Description = dto.Description;
            if (dto.Link != null) banner.Link = dto.Link;
            if (dto.Type.HasValue) banner.Type = dto.Type.Value;
            if (dto.DisplayOrder.HasValue) banner.DisplayOrder = dto.DisplayOrder.Value;
            if (dto.IsActive.HasValue) banner.IsActive = dto.IsActive.Value;

            if (image != null && image.Length > 0)
            {
                // Get WebRootPath or fallback to ContentRootPath/wwwroot
                var webRootPath = _environment.WebRootPath;
                if (string.IsNullOrEmpty(webRootPath))
                {
                    // Fallback: use ContentRootPath and create wwwroot if needed
                    webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                    if (!Directory.Exists(webRootPath))
                    {
                        Directory.CreateDirectory(webRootPath);
                    }
                }

                // Delete old image if exists
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    try
                    {
                        // Extract filename from path (handle both "filename.jpg" and "images/banners/filename.jpg")
                        var oldFileName = banner.ImageUrl.Contains("/") 
                            ? Path.GetFileName(banner.ImageUrl) 
                            : banner.ImageUrl;
                        var oldImagePath = Path.Combine(webRootPath, "images", "banners", oldFileName);
                        if (File.Exists(oldImagePath))
                        {
                            FileHelper.DeleteFile(oldImagePath);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log but don't fail if old image deletion fails
                        Console.WriteLine($"Warning: Could not delete old image: {ex.Message}");
                    }
                }

                // Save new image
                var uploadsFolder = Path.Combine(webRootPath, "images", "banners");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var newFileName = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
                // Store the relative path for serving static files
                banner.ImageUrl = $"images/banners/{newFileName}";
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<BannerDTO>(banner);
        }

        public async Task<bool> DeleteBannerAsync(int id)
        {
            var banner = await _context.Banners.FindAsync(id);
            if (banner == null) return false;

            // Delete image file if exists
            if (!string.IsNullOrEmpty(banner.ImageUrl))
            {
                try
                {
                    // Get WebRootPath or fallback to ContentRootPath/wwwroot
                    var webRootPath = _environment.WebRootPath;
                    if (string.IsNullOrEmpty(webRootPath))
                    {
                        webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
                    }

                    if (!string.IsNullOrEmpty(webRootPath))
                    {
                        var imagePath = Path.Combine(webRootPath, "images", "banners", banner.ImageUrl);
                        if (File.Exists(imagePath))
                        {
                            FileHelper.DeleteFile(imagePath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log but don't fail if image deletion fails
                    Console.WriteLine($"Warning: Could not delete banner image: {ex.Message}");
                }
            }

            _context.Banners.Remove(banner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<FrontBannerResponse>> GetActiveBannersAsync()
        {
            var banners = await _context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<FrontBannerResponse>>(banners);
        }

        public async Task<PaginationResponse<FrontBannerResponse>> GetActiveBannersPaginatedAsync(int page = 1, int pageSize = 10)
        {
            var query = _context.Banners
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder);

            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginationResponse<FrontBannerResponse>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = _mapper.Map<List<FrontBannerResponse>>(items)
            };
        }

        public async Task<List<BannerDTO>> GetMiddleBannersAsync()
        {
            var banners = await _context.Banners
                .Where(b => b.IsActive && b.Type == BannerType.Middle)
                .OrderBy(b => b.DisplayOrder)
                .ToListAsync();

            return _mapper.Map<List<BannerDTO>>(banners);
        }
    }
}
