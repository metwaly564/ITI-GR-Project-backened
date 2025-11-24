using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO model);
        Task<AuthResponseDTO> LoginAsync(LoginDTO model);
        Task<UserDTO> GetUserProfileAsync(string userId);
        Task<bool> UpdateUserProfileAsync(string userId, UserDTO model);
    }
}
