using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
using ITI.Pharmacy.API.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CityService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AdvancedApiResponse<List<CityDTO>>> GetAllCitiesAsync()
        {
            var cities = await _context.Cities
                .Include(c => c.Zones)
                .Where(c => c.IsActive)
                .ToListAsync();

            var dtos = cities.Select(c => new CityDTO
            {
                Id = c.Id,
                Name = c.Name,
                IsActive = c.IsActive,
                Zones = c.Zones.Where(z => z.IsActive).Select(z => new ZoneDTO
                {
                    Id = z.Id,
                    Name = z.Name,
                    DeliveryFee = z.DeliveryFee,
                    IsActive = z.IsActive
                }).ToList()
            }).ToList();

            return AdvancedApiResponse<List<CityDTO>>.CreateSuccess(dtos);
        }

        public async Task<AdvancedApiResponse<CityDTO>> GetCityByIdAsync(int id)
        {
            var city = await _context.Cities
                .Include(c => c.Zones)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (city == null)
                return AdvancedApiResponse<CityDTO>.CreateNotFound("City");

            var dto = new CityDTO
            {
                Id = city.Id,
                Name = city.Name,
                IsActive = city.IsActive,
                Zones = city.Zones.Where(z => z.IsActive).Select(z => new ZoneDTO
                {
                    Id = z.Id,
                    Name = z.Name,
                    DeliveryFee = z.DeliveryFee,
                    IsActive = z.IsActive
                }).ToList()
            };

            return AdvancedApiResponse<CityDTO>.CreateSuccess(dto);
        }

        public async Task<AdvancedApiResponse<List<ZoneDTO>>> GetZonesByCityAsync(int cityId)
        {
            var zones = await _context.Zones
                .Where(z => z.CityId == cityId && z.IsActive)
                .ToListAsync();

            var dtos = zones.Select(z => new ZoneDTO
            {
                Id = z.Id,
                Name = z.Name,
                DeliveryFee = z.DeliveryFee,
                IsActive = z.IsActive
            }).ToList();

            return AdvancedApiResponse<List<ZoneDTO>>.CreateSuccess(dtos);
        }
    }
}
