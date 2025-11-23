using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IAdminAnalyticsService
    {
        Task<AnalyticsDTO> GetAnalyticsAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
        Task<List<OrderTrendDTO>> GetRevenueTrendAsync(int days = 30);
        Task<List<ProductSalesDTO>> GetTopSellingProductsAsync(int top = 10, DateTime? startDate = null, DateTime? endDate = null);
    }
}