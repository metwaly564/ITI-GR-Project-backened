using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetUserCartAsync(string userId);
        Task<CartDTO> AddToCartAsync(string userId, AddToCartDTO dto);
        Task<CartDTO> UpdateCartItemAsync(string userId, int itemId, UpdateCartItemDTO dto);
        Task<bool> RemoveFromCartAsync(string userId, int itemId);
        Task<bool> ClearCartAsync(string userId);
    }
}
