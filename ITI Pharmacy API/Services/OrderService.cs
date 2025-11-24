using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Repositories;

namespace ITI.Pharmacy.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<AdvancedApiResponse<OrderDTO>> CreateOrderAsync(CreateOrderDTO dto, IFormFile? prescriptionImage)
        {
            try
            {
                // هنا إذا كنت تستخدم ملفات صور، يمكنك تعديل DTO لإضافة PrescriptionImageUrl
                var order = await _repository.CreateOrderAsync("mock-user-id", dto);
                return AdvancedApiResponse<OrderDTO>.CreateSuccess(order);
            }
            catch (Exception ex)
            {
                return AdvancedApiResponse<OrderDTO>.CreateError($"Failed to create order: {ex.Message}");
            }
        }

        public async Task<AdvancedApiResponse<OrderDTO>> GetOrderByIdAsync(int id)
        {
            var order = await _repository.GetOrderByIdAsync(id);
            if (order == null)
                return AdvancedApiResponse<OrderDTO>.CreateNotFound("Order");

            return AdvancedApiResponse<OrderDTO>.CreateSuccess(order);
        }

        public async Task<AdvancedApiResponse<List<OrderDTO>>> GetAllOrdersAsync()
        {
            var orders = await _repository.GetAllOrdersAsync();
            return AdvancedApiResponse<List<OrderDTO>>.CreateSuccess(orders);
        }

        //  Repository
        public async Task<AdvancedApiResponse<bool>> CancelOrderAsync(int id, string userId)
        {
            var result = await _repository.CancelOrderAsync(id, userId);
            return result
                ? AdvancedApiResponse<bool>.CreateSuccess(true)
                : AdvancedApiResponse<bool>.CreateError("Failed to cancel order");
        }

        public async Task<AdvancedApiResponse<List<OrderDTO>>> GetOrdersByBranchAsync(int branchId)
        {
            var orders = await _repository.GetOrdersByBranchAsync(branchId);
            return AdvancedApiResponse<List<OrderDTO>>.CreateSuccess(orders);
        }
    }
}
