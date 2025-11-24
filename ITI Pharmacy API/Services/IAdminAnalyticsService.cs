using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface IAdminAnalyticsService
    {
        Task<AnalyticsDTO> GetAnalyticsAsync(DateTime? startDate, DateTime? endDate);
        Task<DashboardStatsDTO> GetDashboardStatsAsync();
        Task<List<OrderTrendDTO>> GetRevenueTrendAsync(int days);
        Task<List<ProductSalesDTO>> GetTopSellingProductsAsync(int top, DateTime? startDate, DateTime? endDate);
        Task<StockAlertsDTO> GetStockAlertsAsync();
    }
}
