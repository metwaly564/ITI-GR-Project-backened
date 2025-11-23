using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace ITI.Pharmacy.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                    .ThenInclude(ps => ps.Branch)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            return product != null ? _mapper.Map<ProductDTO>(product) : null;
        }

        public async Task<ProductDTO?> CreateProductAsync(CreateProductDTO dto, IFormFile? image)
        {
            try
            {
                var product = new Product
                {
                    Name = dto.ProductName,
                    ImageUrl = dto.ProductImage,
                    Price = dto.Price,
                    Description = dto.Description,
                    IsPrescriptionRequired = dto.IsPrescriptionRequired,
                 
                    CategoryId = dto.CategoryId,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return _mapper.Map<ProductDTO>(product);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ProductDTO?> UpdateProductAsync(int id, UpdateProductDTO dto, IFormFile? image)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductStocks)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null) return null;

    
                if (!string.IsNullOrEmpty(dto.ProductName))
                    product.Name = dto.ProductName;

                if (!string.IsNullOrEmpty(dto.ProductImage))
                    product.ImageUrl = dto.ProductImage;

                if (dto.Price.HasValue)
                    product.Price = dto.Price.Value;

                if (!string.IsNullOrEmpty(dto.Description))
                    product.Description = dto.Description;

                if (dto.IsPrescriptionRequired.HasValue)
                    product.IsPrescriptionRequired = dto.IsPrescriptionRequired.Value;

                if (dto.CategoryId.HasValue)
                    product.CategoryId = dto.CategoryId.Value;

                if (dto.IsActive.HasValue)
                    product.IsActive = dto.IsActive.Value;

                product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return _mapper.Map<ProductDTO>(product);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;

                product.IsActive = false;
                product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<List<ProductDTO>> GetFeaturedProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)

                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive && p.CategoryId == categoryId)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<PaginationResponse<ProductDTO>> GetProductsPaginatedAsync(int page, int pageSize, string? search = null)
        {
            // التحقق من المدخلات
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                    .ThenInclude(ps => ps.Branch)
                .Where(p => p.IsActive)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var productDTOs = _mapper.Map<List<ProductDTO>>(products);

            return new PaginationResponse<ProductDTO>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = productDTOs
            };
        }

        public async Task<List<ProductDTO>> SearchProductsAsync(string searchTerm, int? categoryId = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive &&
                           (p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)));

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            var products = await query
                .OrderBy(p => p.Name)
                .Take(20)
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<bool> ToggleFeaturedStatusAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;

          
                product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> GetProductsCountAsync(bool? isActive = null)
        {
            var query = _context.Products.AsQueryable();

            if (isActive.HasValue)
            {
                query = query.Where(p => p.IsActive == isActive.Value);
            }

            return await query.CountAsync();
        }
    }
}