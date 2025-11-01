using AutoMapper;
using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.Models;
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

        public async Task<List<CityDTO>> GetAllCitiesAsync()
        {
            var cities = await _context.Cities
                .Include(c => c.Zones)
                .Where(c => c.IsActive)
                .ToListAsync();

            return cities.Select(c => new CityDTO
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
        }

        public async Task<CityDTO> GetCityByIdAsync(int id)
        {
            var city = await _context.Cities
                .Include(c => c.Zones)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);

            if (city == null) return null;

            return new CityDTO
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
        }

        public async Task<List<ZoneDTO>> GetZonesByCityAsync(int cityId)
        {
            var zones = await _context.Zones
                .Where(z => z.CityId == cityId && z.IsActive)
                .ToListAsync();

            return zones.Select(z => new ZoneDTO
            {
                Id = z.Id,
                Name = z.Name,
                DeliveryFee = z.DeliveryFee,
                IsActive = z.IsActive
            }).ToList();
        }
    }
}