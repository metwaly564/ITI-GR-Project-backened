using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderDTO> CreateOrderAsync(string userId, CreateOrderDTO dto);
        Task<List<OrderDTO>> GetUserOrdersAsync(string userId);
        Task<OrderDTO> GetOrderByIdAsync(int id);
        Task<OrderDTO> GetUserOrderByIdAsync(int id, string userId);
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<List<OrderDTO>> GetOrdersByBranchAsync(int branchId);
        Task<bool> CancelOrderAsync(int id, string userId);
        Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDTO dto);
    }
}