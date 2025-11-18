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
        private readonly IExcelService _excelService;

        public AdminAnalyticsController(IAdminAnalyticsService analyticsService, IExcelService excelService)
        {
            _analyticsService = analyticsService;
            _excelService = excelService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAnalytics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
      
            if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                return BadRequest("StartDate cannot be after EndDate.");

            var analytics = await _analyticsService.GetAnalyticsAsync(startDate, endDate);
            return Ok(analytics);
        }

     
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var stats = await _analyticsService.GetDashboardStatsAsync();
            return Ok(stats);
        }

   
        [HttpGet("reports/products")]
        public IActionResult GenerateProductsReport()
        {
            var report = _excelService.GenerateProductsReport();
            return File(report,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "ProductsReport.xlsx");
        }

 
    
        [HttpGet("reports/orders")]
        public IActionResult GenerateOrdersReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
                return BadRequest("StartDate cannot be after EndDate.");

            var report = _excelService.GenerateOrdersReport(startDate, endDate);

            return File(report,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "OrdersReport.xlsx");
        }
    }
}
