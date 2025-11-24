using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface ICityService
    {
        Task<List<CityDTO>> GetAllCitiesAsync();
        Task<CityDTO> GetCityByIdAsync(int id);
        Task<List<ZoneDTO>> GetZonesByCityAsync(int cityId);
    }
}
