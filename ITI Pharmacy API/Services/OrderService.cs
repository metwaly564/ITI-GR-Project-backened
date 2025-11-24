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

        private async Task<T> ExecuteAsync<T>(Func<Task<T>> action, string errorMessage)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        public Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId), "UserId is required.");

            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Order data is required.");

            if (dto.Items == null || !dto.Items.Any())
                throw new ArgumentException("Order must contain at least one item.", nameof(dto));

            foreach (var item in dto.Items)
            {
                if (item.ProductId <= 0)
                    throw new ArgumentException("Invalid ProductId in order items.", nameof(dto));
                if (item.Quantity <= 0)
                    throw new ArgumentException("Quantity must be greater than 0 in order items.", nameof(dto));
            }

            return ExecuteAsync(() => _orderRepository.CreateOrderAsync(userId, dto),
                $"Error creating order for user {userId}");
        }

        public Task<List<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            return ExecuteAsync(() => _orderRepository.GetUserOrdersAsync(userId),
                $"Error getting orders for user {userId}");
        }

        public Task<OrderDTO> GetOrderByIdAsync(int id, string userId = null)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid order id.", nameof(id));

            return ExecuteAsync(() =>
                string.IsNullOrWhiteSpace(userId)
                    ? _orderRepository.GetOrderByIdAsync(id)
                    : _orderRepository.GetUserOrderByIdAsync(id, userId),
                $"Error getting order {id}");
        }

        public Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            return ExecuteAsync(() => _orderRepository.GetAllOrdersAsync(), "Error getting all orders");
        }

        public Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId)
        {
            if (branchId <= 0)
                throw new ArgumentException("Invalid branch id.", nameof(branchId));

            return ExecuteAsync(() => _orderRepository.GetOrdersByBranchAsync(branchId),
                $"Error getting orders for branch {branchId}");
        }

        public Task<bool> CancelOrderAsync(int id, string userId)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid order id.", nameof(id));

            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            return ExecuteAsync(() => _orderRepository.CancelOrderAsync(id, userId),
                $"Error cancelling order {id} for user {userId}");
        }

        public Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid order id.", nameof(id));

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Status))
                throw new ArgumentException("Status is required.", nameof(dto));

            return ExecuteAsync(() => _orderRepository.UpdateOrderStatusAsync(id, dto),
                $"Error updating status for order {id}");
        }
    }
}
