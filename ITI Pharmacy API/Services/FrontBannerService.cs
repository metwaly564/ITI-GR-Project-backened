using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class FrontBannerService : IFrontBannerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public FrontBannerService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<FrontBannerResponse>> GetActiveBannersAsync(int page = 1, int pageSize = 10)
        {
            var query = _context.Banners
                .AsNoTracking()
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedItems = _mapper.Map<List<FrontBannerResponse>>(items);

            return new PaginationResponse<FrontBannerResponse>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = mappedItems
            };
        }
    }
}
