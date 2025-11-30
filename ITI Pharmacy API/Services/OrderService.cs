using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
        {
            // TODO: Implement order creation logic
            throw new NotImplementedException("OrderService.CreateOrderAsync is not yet implemented");
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id, string? userId = null)
        {
            var query = _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (userId != null)
            {
                query = query.Where(o => o.UserId == userId);
            }

            var order = await query.FirstOrDefaultAsync(o => o.Id == id);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId)
        {
            var orders = await _context.Orders
                .Include(o => o.Branch)
                .Include(o => o.User)
                .Where(o => o.BranchId == branchId)
                .ToListAsync();

            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
                return false;

            if (order.Status == "Delivered" || order.Status == "Cancelled")
                return false;

            order.Status = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return false;

            order.Status = dto.Status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
