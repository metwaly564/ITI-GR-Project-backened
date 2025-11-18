using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface ICityService
    {
        Task<List<CityDTO>> GetAllCitiesAsync();
        Task<CityDTO> GetCityByIdAsync(int id);
        Task<List<ZoneDTO>> GetZonesByCityAsync(int cityId);
    }
}