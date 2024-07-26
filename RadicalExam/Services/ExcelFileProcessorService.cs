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
                        int cellCount = row.LastCellNum;
                        var record = new ExcelRecord();
                        for (int iColumn = 0; iColumn < cellCount; iColumn++)
                        {
                            ICell cell = row.GetCell(iColumn);
                            if (cell is not null)
                            {
                                string cellValue = cell.ToString();
                                record.Cells.Add(cellValue);
                            }
                            else
                            {
                                record.Cells.Add("");
                            }
                        }
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
