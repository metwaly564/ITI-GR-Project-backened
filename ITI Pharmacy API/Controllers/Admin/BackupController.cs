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
    public class BackupController : ControllerBase
    {
        private readonly IBackupService _backupService;

        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        /// <summary>
        /// Import backup data (categories and products) from JSON
        /// Creates categories if they don't exist
        /// Creates/updates products with images
        /// </summary>
        [HttpPost("import")]
        public async Task<ActionResult<AdvancedApiResponse<BackupImportResponseDTO>>> ImportBackupData(
            [FromBody] List<BackupCategoryDTO> backupData)
        {
            try
            {
                if (backupData == null || !backupData.Any())
                {
                    return BadRequest(new AdvancedApiResponse<BackupImportResponseDTO>
                    {
                        Success = false,
                        Message = "Backup data is required",
                        Data = null,
                        Timestamp = DateTime.UtcNow
                    });
                }

                var result = await _backupService.ImportBackupDataAsync(backupData);

                return Ok(new AdvancedApiResponse<BackupImportResponseDTO>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result,
                    Errors = result.Errors.Any() ? result.Errors : null,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new AdvancedApiResponse<BackupImportResponseDTO>
                {
                    Success = false,
                    Message = "Error importing backup data",
                    Errors = new List<string> { ex.Message },
                    Data = null,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }
}
