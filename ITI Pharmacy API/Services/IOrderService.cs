using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Responses;

public interface IOrderService
{
    Task<AdvancedApiResponse<OrderDTO>> CreateOrderAsync(CreateOrderDTO dto, IFormFile? prescriptionImage);
    Task<AdvancedApiResponse<OrderDTO>> GetOrderByIdAsync(int id);
    Task<AdvancedApiResponse<List<OrderDTO>>> GetAllOrdersAsync();

  
    Task<AdvancedApiResponse<bool>> CancelOrderAsync(int id, string userId);
    Task<AdvancedApiResponse<List<OrderDTO>>> GetOrdersByBranchAsync(int branchId);
}
