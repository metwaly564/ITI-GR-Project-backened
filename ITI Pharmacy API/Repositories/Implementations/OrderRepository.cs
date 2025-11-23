using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ITI.Pharmacy.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ApplicationDbContext context, ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // TODO: Implement all methods (???? ????? ?????? ?? NotImplemented)
        public async Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDTO> GetUserOrderByIdAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto)
        {
            throw new NotImplementedException();
        }
    }
}