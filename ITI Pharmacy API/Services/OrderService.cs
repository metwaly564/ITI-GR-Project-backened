using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Repositories;
using Microsoft.Extensions.Logging;

namespace ITI.Pharmacy.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _orderRepository.CreateOrderAsync(userId, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            try
            {
                return await _orderRepository.GetUserOrdersAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for user {UserId}", userId);
                throw;
            }
        }

        public async Task<OrderDTO> GetOrderByIdAsync(int id, string userId = null)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return await _orderRepository.GetOrderByIdAsync(id);
                }
                else
                {
                    return await _orderRepository.GetUserOrderByIdAsync(id, userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order {OrderId}", id);
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepository.GetAllOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders");
                throw;
            }
        }

        public async Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId)
        {
            try
            {
                return await _orderRepository.GetOrdersByBranchAsync(branchId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders for branch {BranchId}", branchId);
                throw;
            }
        }

        public async Task<bool> CancelOrderAsync(int id, string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            try
            {
                return await _orderRepository.CancelOrderAsync(id, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId} for user {UserId}", id, userId);
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            try
            {
                return await _orderRepository.UpdateOrderStatusAsync(id, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order {OrderId}", id);
                throw;
            }
        }
    }
}