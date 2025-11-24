using AutoMapper;
using ITI.Pharmacy.Core.DTOs;
using ITI.Pharmacy.Core.Entities;
using ITI.Pharmacy.Core.Interfaces.Services;
using ITI.Pharmacy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.Infrastructure.Services
{
    public class PharmacyBranchService : IPharmacyBranchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PharmacyBranchService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

   
        public async Task<List<PharmacyBranchDTO>> GetAllBranchesAsync()
        {
            var branches = await _context.PharmacyBranches
                .Where(b => b.IsActive)
                .ToListAsync();

            return _mapper.Map<List<PharmacyBranchDTO>>(branches);
        }

        public async Task<PharmacyBranchDTO> GetBranchByIdAsync(int id)
        {
            var branch = await _context.PharmacyBranches
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);

            return _mapper.Map<PharmacyBranchDTO>(branch);
        }

        public async Task<PharmacyBranchDTO> CreateBranchAsync(CreatePharmacyBranchDTO dto)
        {
            var branch = _mapper.Map<PharmacyBranch>(dto);
            _context.PharmacyBranches.Add(branch);
            await _context.SaveChangesAsync();
            return _mapper.Map<PharmacyBranchDTO>(branch);
        }

        public async Task<PharmacyBranchDTO> UpdateBranchAsync(int id, CreatePharmacyBranchDTO dto)
        {
            var branch = await _context.PharmacyBranches.FindAsync(id);
            if (branch == null) return null;

            _mapper.Map(dto, branch);
            await _context.SaveChangesAsync();
            return _mapper.Map<PharmacyBranchDTO>(branch);
        }

        public async Task<bool> DeleteBranchAsync(int id)
        {
            var branch = await _context.PharmacyBranches.FindAsync(id);
            if (branch == null) return false;

            branch.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
       public async Task<List<ProductStockDTO>> GetBranchStockAsync(int branchId)
        {
            var stock = await _context.ProductStocks
                .Include(ps => ps.Product)
                .Include(ps => ps.Branch)
                .Where(ps => ps.BranchId == branchId)
                .ToListAsync();

            return stock.Select(ps => new ProductStockDTO
            {
                ProductId = ps.ProductId,
                ProductName = ps.Product.Name,
                BranchId = ps.BranchId,
                BranchName = ps.Branch.Name,
                Quantity = ps.Quantity,
                MinimumStockLevel = ps.MinimumStockLevel,
                MaximumStockLevel = ps.MaximumStockLevel,
                LastRestocked = ps.LastRestocked
            }).ToList();
        }

        public async Task<ProductStockDTO> UpdateProductStockAsync(UpdateStockDTO dto)
        {
            var stock = await _context.ProductStocks
                .Include(ps => ps.Product)
                .Include(ps => ps.Branch)
                .FirstOrDefaultAsync(ps => ps.ProductId == dto.ProductId && ps.BranchId == dto.BranchId);

            if (stock == null)
            {
                stock = new ProductStock
                {
                    ProductId = dto.ProductId,
                    BranchId = dto.BranchId,
                    Quantity = dto.Quantity,
                    LastRestocked = DateTime.UtcNow
                };
                _context.ProductStocks.Add(stock);
            }
            else
            {
                stock.Quantity = dto.Quantity;
                stock.LastRestocked = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new ProductStockDTO
            {
                ProductId = stock.ProductId,
                ProductName = stock.Product.Name,
                BranchId = stock.BranchId,
                BranchName = stock.Branch.Name,
                Quantity = stock.Quantity,
                MinimumStockLevel = stock.MinimumStockLevel,
                MaximumStockLevel = stock.MaximumStockLevel,
                LastRestocked = stock.LastRestocked
            };
        }

        public async Task<List<StockTransferDTO>> GetBranchTransfersAsync(int branchId)
        {
            var transfers = await _context.StockTransfers
                .Include(st => st.Product)
                .Include(st => st.FromBranch)
                .Include(st => st.ToBranch)
                .Where(st => st.FromBranchId == branchId || st.ToBranchId == branchId)
                .OrderByDescending(st => st.RequestedAt)
                .ToListAsync();

            return transfers.Select(st => new StockTransferDTO
            {
                Id = st.Id,
                ProductId = st.ProductId,
                ProductName = st.Product.Name,
                FromBranchId = st.FromBranchId,
                FromBranchName = st.FromBranch.Name,
                ToBranchId = st.ToBranchId,
                ToBranchName = st.ToBranch.Name,
                Quantity = st.Quantity,
                Reason = st.Reason,
                Status = st.Status,
                RequestedAt = st.RequestedAt
            }).ToList();
        }

 
        public async Task<PaginationResponse<PharmacyBranchDTO>> FilterBranchesWithPaginationAsync(
            int page, int pageSize,
            string? search = null,
            string sortBy = "Name",
            string sortDirection = "ASC")
        {
            var query = _context.PharmacyBranches
                .Where(b => b.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(b => b.Name.Contains(search) || b.Address.Contains(search));

            var totalCount = await query.CountAsync();

            query = (sortBy.ToLower(), sortDirection.ToUpper()) switch
            {
                ("name", "asc") => query.OrderBy(b => b.Name),
                ("name", "desc") => query.OrderByDescending(b => b.Name),
                ("address", "asc") => query.OrderBy(b => b.Address),
                ("address", "desc") => query.OrderByDescending(b => b.Address),
                _ => query.OrderBy(b => b.Name)
            };

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<PharmacyBranchDTO>
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = _mapper.Map<List<PharmacyBranchDTO>>(items)
            };
        }
    }
}
