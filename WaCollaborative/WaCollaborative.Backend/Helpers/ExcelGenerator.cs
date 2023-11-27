using OfficeOpenXml;
using WaCollaborative.Backend.Helpers.Interfaces;
using WaCollaborative.Shared.DTOs;
using WaCollaborative.Shared.Entities;

namespace WaCollaborative.Backend.Helpers
{
    public class ExcelGenerator : IExcelGenerator
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExcelGenerator(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(string filePath, string fileName)> GenerateExcelFileAsync(List<CollaborativeDemandDTO> collaborativeDemandDTOs)
        {
            var fileDownloadName = $"CollaborativeDemands{DateTime.Now:yyyyMMddHHmmss}.xlsx";

            var excelFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "ExcelFiles", fileDownloadName);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("CollaborativeDemands");

                var headers = collaborativeDemandDTOs.First().GetType().GetProperties().Select(property => property.Name).ToArray();

                for (var i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                for (var i = 0; i < collaborativeDemandDTOs.Count; i++)
                {
                    var rowData = collaborativeDemandDTOs[i];
                    var properties = rowData.GetType().GetProperties();
                    for (var j = 0; j < properties.Length; j++)
                    {
                        worksheet.Cells[i + 2, j + 1].Value = properties[j].GetValue(rowData);
                    }
                }

                FileInfo excelFile = new FileInfo(excelFilePath);
                package.SaveAs(excelFile);
            }

            return (excelFilePath, fileDownloadName);
        }
    }
}