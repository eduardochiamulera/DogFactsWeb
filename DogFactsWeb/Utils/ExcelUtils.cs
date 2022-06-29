using DogFactsWeb.Models;
using ExcelDataReader;
using OfficeOpenXml;

namespace DogFactsWeb.Utils
{
    public class ExcelUtils
    {
        private const string FILE_PATH = @"C:\excell\";
        private const string FILE_NAME = "DogFacts";
        private const string FILE_EXTENSION = ".xlsx";
        private const int COLUMN_NUMBER = 1;

        static ExcelUtils() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        public static IList<string> GetExcelData(IFormFile file)
        {
            var list = new List<string>();

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                ms.Position = 0;
                using (var reader = ExcelReaderFactory.CreateReader(ms))
                {
                    while (reader.Read())
                    {
                        var id = reader.GetValue(0).ToString();
                        if (!string.IsNullOrWhiteSpace(id))
                            list.Add(id);
                    }
                }
            }

            return list;
        }

        public static GenericResponseVM GenerateExcel(IList<DogFactsResponse> data)
        {
            try
            {
                var package = new ExcelPackage();
                var excelWorkbook = package.Workbook;
                var fullPath = $"{FILE_PATH}{FILE_NAME}{DateTime.Now.ToString("dd-mm-yyy HH-MM-ss")}{FILE_EXTENSION}";

                for (int i = 0; i < data.Count(); i++)
                {
                    var workSheet = excelWorkbook.Worksheets.Add($"Sheet{i + 1}");
                    var sheet = excelWorkbook.Worksheets[$"Sheet{i + 1}"];
                    var index = 1;

                    foreach (var fact in data[i].Facts)
                    {
                        sheet.Cells[index, COLUMN_NUMBER].Value = fact;
                        index++;
                    }
                    workSheet.Column(COLUMN_NUMBER).AutoFit();
                }

                if (!Directory.Exists(FILE_PATH))
                    Directory.CreateDirectory(FILE_PATH);

                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                var objFileStream = File.Create(fullPath);
                objFileStream.Close();

                File.WriteAllBytes(fullPath, package.GetAsByteArray());

                return new GenericResponseVM().AddMessage($"Arquivo Gerado com sucesso. O mesmo foi salvo no caminho: {fullPath}");
            }
            catch (Exception ex)
            {
                return new GenericResponseVM().AddError(ex.Message);
            }
        }
    }
}
