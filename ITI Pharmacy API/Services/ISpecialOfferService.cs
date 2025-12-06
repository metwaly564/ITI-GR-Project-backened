using ITI.Pharmacy.API.DTOs;

namespace ITI.Pharmacy.API.Services
{
    public interface ICategoryImportService
    {
        Task<CategoryImportResponseDTO> ImportCategoryAsync(CategoryImportDTO categoryImport);
    }
}
