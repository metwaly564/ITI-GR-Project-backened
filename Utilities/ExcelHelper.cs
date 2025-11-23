using OfficeOpenXml;
using ITI.Pharmacy.API.Models;

namespace ITI.Pharmacy.API.Utilities
{
    public class ExcelHelper
    {
        public static byte[] GenerateProductsReport(List<Product> products)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Products");

            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Name";
            worksheet.Cells[1, 3].Value = "Price";
            worksheet.Cells[1, 4].Value = "Category";
            worksheet.Cells[1, 5].Value = "Prescription Required";
            worksheet.Cells[1, 6].Value = "Active";

            // Data
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                worksheet.Cells[i + 2, 1].Value = product.Id;
                worksheet.Cells[i + 2, 2].Value = product.Name;
                worksheet.Cells[i + 2, 3].Value = product.Price;
                worksheet.Cells[i + 2, 4].Value = product.Category?.Name;
                worksheet.Cells[i + 2, 5].Value = product.IsPrescriptionRequired ? "Yes" : "No";
                worksheet.Cells[i + 2, 6].Value = product.IsActive ? "Yes" : "No";
            }

            worksheet.Cells.AutoFitColumns();
            return package.GetAsByteArray();
        }
    }
}