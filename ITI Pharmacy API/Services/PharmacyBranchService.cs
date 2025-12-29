using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Hubs;
using ITI.Pharmacy.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class PharmacyBranchService : IPharmacyBranchService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public PharmacyBranchService(ApplicationDbContext context, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        // ================== Branch CRUD ==================
        public async Task<List<PharmacyBranchDTO>> GetAllBranchesAsync()
        {
            var branches = await _context.PharmacyBranches
                .Where(b => b.IsActive)
                .ToListAsync();

            return _mapper.Map<List<PharmacyBranchDTO>>(branches);
        }

        public async Task<PharmacyBranchDTO?> GetBranchByIdAsync(int id)
        {
            var branch = await _context.PharmacyBranches
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);

            if (branch == null) return null;

            return _mapper.Map<PharmacyBranchDTO>(branch);
        }

        public async Task<PharmacyBranchDTO> CreateBranchAsync(CreatePharmacyBranchDTO dto)
        {
            var branch = _mapper.Map<PharmacyBranch>(dto);
            _context.PharmacyBranches.Add(branch);
            await _context.SaveChangesAsync();
            return _mapper.Map<PharmacyBranchDTO>(branch);
        }

        public async Task<PharmacyBranchDTO?> UpdateBranchAsync(int id, CreatePharmacyBranchDTO dto)
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

        // ================== Branch Stock ==================
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
                ProductName = ps.Product?.Name ?? string.Empty,
                BranchId = ps.BranchId,
                BranchName = ps.Branch?.Name ?? string.Empty,
                Quantity = ps.Quantity,
                MinimumStockLevel = ps.MinimumStockLevel,
                MaximumStockLevel = ps.MaximumStockLevel,
                // ExpiryDate = ps.ExpiryDate,
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
                    // ExpiryDate = dto.ExpiryDate,
                    LastRestocked = DateTime.UtcNow
                };
                _context.ProductStocks.Add(stock);
            }
            else
            {
                stock.Quantity = dto.Quantity;
                // stock.ExpiryDate = dto.ExpiryDate;
                stock.LastRestocked = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return new ProductStockDTO
            {
                ProductId = stock.ProductId,
                ProductName = stock.Product?.Name ?? string.Empty,
                BranchId = stock.BranchId,
                BranchName = stock.Branch?.Name ?? string.Empty,
                Quantity = stock.Quantity,
                MinimumStockLevel = stock.MinimumStockLevel,
                MaximumStockLevel = stock.MaximumStockLevel,
                // ExpiryDate = stock.ExpiryDate,
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
                ProductName = st.Product?.Name ?? string.Empty,
                FromBranchId = st.FromBranchId,
                FromBranchName = st.FromBranch?.Name ?? string.Empty,
                ToBranchId = st.ToBranchId,
                ToBranchName = st.ToBranch?.Name ?? string.Empty,
                Quantity = st.Quantity,
                Reason = st.Reason,
                Status = st.Status,
                RequestedAt = st.RequestedAt
            }).ToList();
        }

        public async Task<StockTransferDTO?> InitiateTransferAsync(CreateStockTransferDTO dto, string requestedBy)
        {
            var fromStock = await _context.ProductStocks
                .FirstOrDefaultAsync(ps => ps.ProductId == dto.ProductId && ps.BranchId == dto.FromBranchId);
            
            if (fromStock == null || fromStock.Quantity < dto.Quantity)
                return null;

            var transfer = new StockTransfer
            {
                ProductId = dto.ProductId,
                FromBranchId = dto.FromBranchId,
                ToBranchId = dto.ToBranchId,
                Quantity = dto.Quantity,
                Reason = dto.Reason,
                Status = "Pending",
                RequestedAt = DateTime.UtcNow,
                RequestedBy = requestedBy
            };

            _context.StockTransfers.Add(transfer);
            
            // Deduct from source branch immediately or when approved? 
            // Usually it's deducted when approved/completed, but the user wants to trigger notification when initiated.
            
            await _context.SaveChangesAsync();

            // Notify Admin or Receiving Branch? 
            // Let's notify "All" for now as per user request to support All/Specific.
            var product = await _context.Products.FindAsync(dto.ProductId);
            var toBranch = await _context.PharmacyBranches.FindAsync(dto.ToBranchId);
            
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", new {
                title = "New Product Transfer",
                message = $"Transfer of {dto.Quantity} {product?.Name} to {toBranch?.Name} initiated.",
                type = "Transfer"
            });

            return new StockTransferDTO
            {
                Id = transfer.Id,
                ProductId = transfer.ProductId,
                ProductName = product?.Name ?? string.Empty,
                FromBranchId = transfer.FromBranchId,
                ToBranchId = transfer.ToBranchId,
                Quantity = transfer.Quantity,
                Reason = transfer.Reason,
                Status = transfer.Status,
                RequestedAt = transfer.RequestedAt
            };
        }

        // ================== Filter + Pagination ==================
        public async Task<PaginationResponse<PharmacyBranchDTO>> FilterBranchesWithPaginationAsync(
            int page,
            int pageSize,
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
