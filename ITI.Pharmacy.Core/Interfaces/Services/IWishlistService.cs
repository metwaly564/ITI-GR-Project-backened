using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface IWishlistService
    {
        Task<WishlistDTO> GetUserWishlistAsync(string userId);
        Task<bool> AddToWishlistAsync(string userId, int productId);
        Task<bool> RemoveFromWishlistAsync(string userId, int productId);
        Task<bool> ClearWishlistAsync(string userId);
    }
}
