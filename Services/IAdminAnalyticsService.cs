using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IAdminAnalyticsService
    {
        Task<AnalyticsDTO> GetAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetDashboardStatsAsync();
    }
}