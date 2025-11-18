using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IHomeService
    {
        Task<HomeDataDTO> GetHomeDataAsync();
    }
}