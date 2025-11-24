using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface IHomeService
    {
        Task<HomeDataDTO> GetHomeDataAsync();
    }
}
