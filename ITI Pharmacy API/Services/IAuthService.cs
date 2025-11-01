using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO model);
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<UserDTO> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(string userId, UserDTO model);
    }
}