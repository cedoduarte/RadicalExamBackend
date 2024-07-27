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
            _banxicoService.Token = token;
            return Ok(await _banxicoService.GetExchangeRate(startDate, endDate));
        }
    }
}
