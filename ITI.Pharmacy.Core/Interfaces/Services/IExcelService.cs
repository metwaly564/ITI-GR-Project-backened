namespace ITI.Pharmacy.Core.Interfaces.Services
{
    public interface IExcelService
    {
        byte[] GenerateProductsReport();
        byte[] GenerateOrdersReport(DateTime startDate, DateTime endDate);
    }
}
