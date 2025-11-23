using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class HomeService : IHomeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public HomeService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AdvancedApiResponse<PaginationResponse<ProductDTO>>> GetHomeDataAsync(
            int branchId, int page, int limit, string? sort)
        {
            try
            {
                if (page < 1) page = 1;
                if (limit < 1) limit = 10;
                if (limit > 100) limit = 100;

                var query = _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductStocks)
                    .Where(p => p.IsActive)
                    .AsQueryable();

           
                if (branchId > 0)
                {
                    query = query.Where(p => p.ProductStocks.Any(s => s.BranchId == branchId && s.Quantity > 0));
                }

         
       
                var totalCount = await query.CountAsync();

           
                var products = await query
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .ToListAsync();

   
                var productsDTO = _mapper.Map<List<ProductDTO>>(products);

                var pagination = new PaginationResponse<ProductDTO>
                {
                    CurrentPage = page,
                    PageSize = limit,
                    TotalCount = totalCount,
                    Items = productsDTO
                };

          
                return new AdvancedApiResponse<PaginationResponse<ProductDTO>>
                {
                    Success = true,
                    Message = "تم جلب المنتجات بنجاح",
                    Data = pagination,
                    Timestamp = DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
            
                return new AdvancedApiResponse<PaginationResponse<ProductDTO>>
                {
                    Success = false,
                    Message = "حدث خطأ أثناء جلب البيانات",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                };
            }
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sort, int branchId)
        {
            return sort?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.Price),
                "price_desc" => query.OrderByDescending(p => p.Price),
                "name_asc" => query.OrderBy(p => p.Name),
                "name_desc" => query.OrderByDescending(p => p.Name),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                "popular" => query.OrderByDescending(p => p.ProductStocks
                    .Where(s => branchId == 0 || s.BranchId == branchId)
                    .Sum(s => s.Quantity)),
              
                _ => query.OrderBy(p => p.Name) 
            };
        }
    }
}