using ITI.Pharmacy.API.DTOs;
using ITI.Pharmacy.API.DTOs.Responses;
using ITI.Pharmacy.API.Models.Responses;
using ITI.Pharmacy.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI.Pharmacy.API.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminAnalyticsController : ControllerBase
    {
        private readonly IAdminAnalyticsService _analyticsService;

        public AdminAnalyticsController(IAdminAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // GET: api/admin/adminanalytics?startDate=2025-01-01&endDate=2025-01-31
        [HttpGet]
        public async Task<ActionResult<AdvancedApiResponse<AnalyticsDTO>>> GetAnalytics(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            try
            {
                var analytics = await _analyticsService.GetAnalyticsAsync(startDate, endDate);
                return Ok(new AdvancedApiResponse<AnalyticsDTO>
                {
                    Success = true,
                    Message = "Analytics data retrieved successfully",
                    Data = analytics,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<AnalyticsDTO>
                {
                    Success = false,
                    Message = "Error retrieving analytics data",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        // GET: api/admin/adminanalytics/dashboard
        [HttpGet("dashboard")]
        public async Task<ActionResult<AdvancedApiResponse<DashboardStatsDTO>>> GetDashboardStats()
        {
            try
            {
                var stats = await _analyticsService.GetDashboardStatsAsync();
                return Ok(new AdvancedApiResponse<DashboardStatsDTO>
                {
                    Success = true,
                    Message = "Dashboard stats retrieved successfully",
                    Data = stats,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<DashboardStatsDTO>
                {
                    Success = false,
                    Message = "Error retrieving dashboard stats",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        // GET: api/admin/adminanalytics/revenue-trend?days=30
        [HttpGet("revenue-trend")]
        public async Task<ActionResult<AdvancedApiResponse<List<OrderTrendDTO>>>> GetRevenueTrend([FromQuery] int days = 30)
        {
            try
            {
                if (days <= 0 || days > 365)
                {
                    return BadRequest(new AdvancedApiResponse<List<OrderTrendDTO>>
                    {
                        Success = false,
                        Message = "Days parameter must be between 1 and 365",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                var trend = await _analyticsService.GetRevenueTrendAsync(days);
                return Ok(new AdvancedApiResponse<List<OrderTrendDTO>>
                {
                    Success = true,
                    Message = "Revenue trend retrieved successfully",
                    Data = trend,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<List<OrderTrendDTO>>
                {
                    Success = false,
                    Message = "Error retrieving revenue trend",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        // GET: api/admin/adminanalytics/top-products?top=10&startDate=2025-01-01&endDate=2025-01-31
        [HttpGet("top-products")]
        public async Task<ActionResult<AdvancedApiResponse<List<ProductSalesDTO>>>> GetTopSellingProducts(
            [FromQuery] int top = 10,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            try
            {
                if (top <= 0 || top > 50)
                {
                    return BadRequest(new AdvancedApiResponse<List<ProductSalesDTO>>
                    {
                        Success = false,
                        Message = "Top parameter must be between 1 and 50",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                var products = await _analyticsService.GetTopSellingProductsAsync(top, startDate, endDate);
                return Ok(new AdvancedApiResponse<List<ProductSalesDTO>>
                {
                    Success = true,
                    Message = "Top selling products retrieved successfully",
                    Data = products,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<List<ProductSalesDTO>>
                {
                    Success = false,
                    Message = "Error retrieving top selling products",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        // GET: api/admin/adminanalytics/stock-alerts
        [HttpGet("stock-alerts")]
        public async Task<ActionResult<AdvancedApiResponse<object>>> GetStockAlerts()
        {
            try
            {
       /// may be add notification
                var lowStockCount = 0;
                var outOfStockCount = 0; 

                return Ok(new AdvancedApiResponse<object>
                {
                    Success = true,
                    Message = "Stock alerts retrieved successfully",
                    Data = new
                    {
                        LowStockAlerts = lowStockCount,
                        OutOfStockAlerts = outOfStockCount,
                        TotalAlerts = lowStockCount + outOfStockCount
                    },
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new AdvancedApiResponse<object>
                {
                    Success = false,
                    Message = "Error retrieving stock alerts",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}