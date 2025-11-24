using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models.Responses;

namespace ITI.Pharmacy.API.Services
{
    public interface ICityService
    {
        Task<AdvancedApiResponse<List<CityDTO>>> GetAllCitiesAsync();
        Task<AdvancedApiResponse<CityDTO>> GetCityByIdAsync(int id);
        Task<AdvancedApiResponse<List<ZoneDTO>>> GetZonesByCityAsync(int cityId);
    }
}
