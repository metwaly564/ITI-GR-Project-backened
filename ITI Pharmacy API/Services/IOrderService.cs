using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IOrderService
    {
        Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto);
        Task<List<OrderDTO>> GetUserOrdersAsync(string userId);
        Task<OrderDTO> GetOrderByIdAsync(int id, string userId = null);
        Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto);
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId);
        Task<bool> CancelOrderAsync(int id, string userId);
    }
}
