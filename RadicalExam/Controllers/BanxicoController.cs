using Microsoft.AspNetCore.Mvc;
using RadicalExam.Services;

namespace RadicalExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BanxicoController : Controller
    {
        private readonly IBanxicoService _banxicoService;

        public BanxicoController(IBanxicoService banxicoService)
        {
            _banxicoService = banxicoService;
        }

        [HttpGet("exchange-rate/{startDate}/{endDate}/{token}")]
        public async Task<IActionResult> GetExchangeRate(string startDate, string endDate, string token)
        {
            string format = "yyyy-MM-dd";
            DateTime startDateTime = DateTime.ParseExact(startDate, format, null);
            DateTime endDateTime = DateTime.ParseExact(endDate, format, null);
            if (startDateTime > endDateTime)
            {
                return BadRequest("¡La fecha de inicio debe ser anterior a la fecha de fin!");
            }
            _banxicoService.Token = token;
            return Ok(await _banxicoService.GetExchangeRate(startDate, endDate));
        }
    }
}
