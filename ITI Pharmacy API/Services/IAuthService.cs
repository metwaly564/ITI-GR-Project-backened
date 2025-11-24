using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Responses;

namespace ITI.Pharmacy.API.Services
{
    public interface IAuthService
    {
        Task<AdvancedApiResponse<AuthResponseDTO>> RegisterAsync(RegisterDTO model);
        Task<AdvancedApiResponse<AuthResponseDTO>> LoginAsync(LoginDTO model);
        Task<AdvancedApiResponse<UserDTO>> GetUserProfileAsync(string userId);
        Task<AdvancedApiResponse<bool>> UpdateUserProfileAsync(string userId, UserDTO model);
    }
}
