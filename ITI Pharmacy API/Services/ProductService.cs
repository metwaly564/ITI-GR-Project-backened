using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public ProductService(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _context = context;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .ThenInclude(ps => ps.Branch)
                .Where(p => p.IsActive)
                .ToListAsync();

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                ImageUrl = p.ImageUrl,
                IsPrescriptionRequired = p.IsPrescriptionRequired,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                CreatedAt = p.CreatedAt,
                Stock = p.ProductStocks.Select(ps => new ProductStockDTO
                {
                    ProductId = ps.ProductId,
                    BranchId = ps.BranchId,
                    BranchName = ps.Branch.Name,
                    Quantity = ps.Quantity,
                    MinimumStockLevel = ps.MinimumStockLevel,
                    MaximumStockLevel = ps.MaximumStockLevel,
                    LastRestocked = ps.LastRestocked
                }).ToList()
            }).ToList();
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .ThenInclude(ps => ps.Branch)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

            if (product == null) return null;

            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ImageUrl = product.ImageUrl,
                IsPrescriptionRequired = product.IsPrescriptionRequired,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.Category.Name,
                CreatedAt = product.CreatedAt,
                Stock = product.ProductStocks.Select(ps => new ProductStockDTO
                {
                    ProductId = ps.ProductId,
                    BranchId = ps.BranchId,
                    BranchName = ps.Branch.Name,
                    Quantity = ps.Quantity,
                    MinimumStockLevel = ps.MinimumStockLevel,
                    MaximumStockLevel = ps.MaximumStockLevel,
                    LastRestocked = ps.LastRestocked
                }).ToList()
            };
        }

        public async Task<List<ProductDTO>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<List<ProductDTO>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<ProductDTO>();

            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive &&
                           (p.Name.Contains(searchTerm) ||
                            p.Description.Contains(searchTerm) ||
                            p.Category.Name.Contains(searchTerm)))
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDTO dto, IFormFile image)
        {
            var product = _mapper.Map<Product>(dto);

            if (image != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                product.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Create stock entries for all branches
            var branches = await _context.PharmacyBranches.Where(b => b.IsActive).ToListAsync();
            foreach (var branch in branches)
            {
                var stock = new ProductStock
                {
                    ProductId = product.Id,
                    BranchId = branch.Id,
                    Quantity = 0,
                    MinimumStockLevel = 10,
                    MaximumStockLevel = 100
                };
                _context.ProductStocks.Add(stock);
            }

            await _context.SaveChangesAsync();
            return await GetProductByIdAsync(product.Id);
        }

        public async Task<ProductDTO> UpdateProductAsync(int id, UpdateProductDTO dto, IFormFile image)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return null;

            if (!string.IsNullOrEmpty(dto.Name)) product.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description)) product.Description = dto.Description;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.DiscountPrice.HasValue) product.DiscountPrice = dto.DiscountPrice.Value;
            if (dto.IsPrescriptionRequired.HasValue) product.IsPrescriptionRequired = dto.IsPrescriptionRequired.Value;
            if (dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;
            if (dto.IsActive.HasValue) product.IsActive = dto.IsActive.Value;

            product.UpdatedAt = DateTime.UtcNow;

            if (image != null)
            {
                // Delete old image
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, "images", "products", product.ImageUrl);
                    FileHelper.DeleteFile(oldImagePath);
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", "products");
                product.ImageUrl = await FileHelper.SaveFileAsync(image, uploadsFolder, new[] { ".jpg", ".jpeg", ".png", ".gif" });
            }

            await _context.SaveChangesAsync();
            return await GetProductByIdAsync(product.Id);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            product.IsActive = false;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductDTO>> GetFeaturedProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive && p.DiscountPrice.HasValue)
                .OrderByDescending(p => p.DiscountPrice)
                .Take(10)
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<List<ProductDTO>> GetProductsWithLowStockAsync(int branchId)
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductStocks)
                .Where(p => p.IsActive &&
                           p.ProductStocks.Any(ps => ps.BranchId == branchId && ps.Quantity <= ps.MinimumStockLevel))
                .ToListAsync();

            return _mapper.Map<List<ProductDTO>>(products);
        }
    }
}