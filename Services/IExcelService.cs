namespace ITI.Pharmacy.API.Services
{
    public interface IExcelService
    {
        byte[] GenerateProductsReport();
        byte[] GenerateOrdersReport(DateTime startDate, DateTime endDate);
    }
}