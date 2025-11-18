using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IWishlistService
    {
        Task<WishlistDTO> GetUserWishlistAsync(string userId);
        Task<bool> AddToWishlistAsync(string userId, int productId);
        Task<bool> RemoveFromWishlistAsync(string userId, int productId);
        Task<bool> ClearWishlistAsync(string userId);
    }
}