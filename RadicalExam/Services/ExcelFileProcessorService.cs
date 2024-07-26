using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RadicalExam.DTOs;

namespace RadicalExam.Services
{
    public interface IExcelFileProcessorService
    {
        ExcelDocument ReadFile(IFormFile inputFile);
    }

    public class ExcelFileProcessorService : IExcelFileProcessorService
    {
        private IWorkbook GetWorkbookFromFile(IFormFile inputFile)
        {
            IWorkbook workbook = null;
            using (var stream = inputFile.OpenReadStream())
            {
                string inputFileName = inputFile.FileName.ToLower();
                if (inputFileName.EndsWith(".xls"))
                {
                    workbook = new HSSFWorkbook(stream);
                }
                else if (inputFileName.EndsWith(".xlsx"))
                {
                    workbook = new XSSFWorkbook(stream);
                }
            }
            return workbook;
        }

        public ExcelDocument ReadFile(IFormFile inputFile)
        {
            try
            {
                var workbook = GetWorkbookFromFile(inputFile);
                if (workbook is null)
                {
                    throw new Exception("The input file is not an Excel file!");
                }
                var document = new ExcelDocument();
                ISheet sheet = workbook.GetSheetAt(0);
                int rowCount = sheet.LastRowNum + 1;
                for (int iRow = 1; iRow < rowCount; iRow++)
                {
                    IRow row = sheet.GetRow(iRow);
                    if (row is not null)
                    {
                        ICell cell = null;
                        var record = new ExcelRecord();

                        cell = row.GetCell(0);
                        record.FirstName = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(1);
                        record.MiddleName = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(2);
                        record.FirstLastName = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(3);
                        record.SecondLastName = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(4);
                        record.Birthdate = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(5);
                        record.RFC = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(6);
                        record.Neighborhood = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(7);
                        record.DelegationOrMunicipality = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(8);
                        record.City = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(9);
                        record.State = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(10);
                        record.ZipCode = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(11);
                        record.Address = cell is not null ? cell.ToString() : "";

                        cell = row.GetCell(12);
                        record.CurrentBalance = cell is not null ? decimal.Parse(cell.ToString()) : 0.0m;

                        cell = row.GetCell(13);
                        record.CreditLimit = cell is not null ? decimal.Parse(cell.ToString()) : 0.0m;

                        cell = row.GetCell(14);
                        record.BalanceDue = cell is not null ? decimal.Parse(cell.ToString()) : 0.0m;

                        record.AvailableBalance = record.CreditLimit - record.CurrentBalance;

                        document.Records.Add(record);
                    }
                }
                return document;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
