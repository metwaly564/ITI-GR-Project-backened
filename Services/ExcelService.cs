using OfficeOpenXml;
using ITI.Pharmacy.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ITI.Pharmacy.API.Services
{
    public class ExcelService : IExcelService
    {
        private readonly ApplicationDbContext _context;

        public ExcelService(ApplicationDbContext context)
        {
            _context = context;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public byte[] GenerateProductsReport()
        {
            var products = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");

            // Headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Description";
            worksheet.Cells[1, 4].Value = "Price";
            worksheet.Cells[1, 5].Value = "Discount Price";
            worksheet.Cells[1, 6].Value = "Category";
            worksheet.Cells[1, 7].Value = "Prescription Required";
            worksheet.Cells[1, 8].Value = "Created At";

            // Data
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                worksheet.Cells[i + 2, 1].Value = product.Id;
                worksheet.Cells[i + 2, 2].Value = product.Name;
                worksheet.Cells[i + 2, 3].Value = product.Description;
                worksheet.Cells[i + 2, 4].Value = product.Price;
                worksheet.Cells[i + 2, 5].Value = product.DiscountPrice;
                worksheet.Cells[i + 2, 6].Value = product.Category?.Name;
                worksheet.Cells[i + 2, 7].Value = product.IsPrescriptionRequired ? "Yes" : "No";
                worksheet.Cells[i + 2, 8].Value = product.CreatedAt.ToString("yyyy-MM-dd");
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }

        public byte[] GenerateOrdersReport(DateTime startDate, DateTime endDate)
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Branch)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToList();

            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Orders");

            // Headers
            worksheet.Cells[1, 1].Value = "Order Number";
            worksheet.Cells[1, 2].Value = "Customer";
            worksheet.Cells[1, 3].Value = "Branch";
            worksheet.Cells[1, 4].Value = "Total Amount";
            worksheet.Cells[1, 5].Value = "Delivery Fee";
            worksheet.Cells[1, 6].Value = "Status";
            worksheet.Cells[1, 7].Value = "Payment Status";
            worksheet.Cells[1, 8].Value = "Created At";

            // Data
            for (int i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                worksheet.Cells[i + 2, 1].Value = order.OrderNumber;
                worksheet.Cells[i + 2, 2].Value = $"{order.User.FirstName} {order.User.LastName}";
                worksheet.Cells[i + 2, 3].Value = order.Branch.Name;
                worksheet.Cells[i + 2, 4].Value = order.TotalAmount;
                worksheet.Cells[i + 2, 5].Value = order.DeliveryFee;
                worksheet.Cells[i + 2, 6].Value = order.Status;
                worksheet.Cells[i + 2, 7].Value = order.PaymentStatus;
                worksheet.Cells[i + 2, 8].Value = order.CreatedAt.ToString("yyyy-MM-dd HH:mm");
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}