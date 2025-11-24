using ITI.Pharmacy.Core.DTOs;

namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface IAdminAnalyticsService
    {
        Task<AnalyticsDTO> GetAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<object> GetDashboardStatsAsync();
    }
}
