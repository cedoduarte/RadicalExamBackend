using Microsoft.AspNetCore.Mvc;
using RadicalExam.Services;

namespace RadicalExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        private readonly IExcelFileProcessorService _excelFileProcessorService;

        public UploadController(IExcelFileProcessorService excelFileProcessorService)
        {
            _excelFileProcessorService = excelFileProcessorService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm(Name = "file")] IFormFile inputFile)
        {
            if (inputFile is null || inputFile.Length == 0)
            {
                return BadRequest("¡No se subió ningún archivo!");
            }
            var result = _excelFileProcessorService.ReadFile(inputFile);
            if (result.Records.Any())
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("¡El archivo de Excel está vacío!");
            }
        }
    }
}
