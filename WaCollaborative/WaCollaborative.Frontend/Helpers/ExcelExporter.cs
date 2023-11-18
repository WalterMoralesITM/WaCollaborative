using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using ClosedXML.Excel;
using System.IO.Packaging;

namespace WaCollaborative.Frontend.Helpers
{
    public class ExcelExporter
    {
        public static void ExportToExcel<T>(IEnumerable<T> data, string fileName)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                string downloadsPath = Path.Combine(wwwrootPath, "Downloads");

                if (!Directory.Exists(downloadsPath))
                {
                    Directory.CreateDirectory(downloadsPath);
                }
                // Combina la ruta de descargas con el nombre del archivo
                string filePath = Path.Combine(downloadsPath, fileName);
                /*OfficeOpenXml*/
                using (var package = new ExcelPackage())
                {
                    var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                    workSheet.Cells.LoadFromCollection(data, true);

                    // Guarda el archivo Excel en la ruta especificada
                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                }                

                Console.WriteLine($"El archivo se ha generado correctamente en: {filePath}");
            }
            catch (OfficeOpenXml.LicenseException ope)
            {
                throw new Exception(ope.Message);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }
    }
}
