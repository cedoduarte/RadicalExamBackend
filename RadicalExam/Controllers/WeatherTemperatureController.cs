using Microsoft.AspNetCore.Mvc;
using RadicalExam.Services;

namespace RadicalExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherTemperatureController : Controller
    {
        private readonly IWeatherTemperatureService _weatherService;
        private readonly IConfiguration _configuration;

        public WeatherTemperatureController(IWeatherTemperatureService weatherService, IConfiguration configuration)
        {
            _weatherService = weatherService;
            _configuration = configuration;
            _weatherService.ApiKey = _configuration["OpenWeatherMapApiKey"].ToString();
        }

        [HttpGet("current-temperature/{cityName}")]
        public async Task<IActionResult> GetCurrentTemperature(string cityName)
        {
            return Ok(await _weatherService.GetTemperature("MX", cityName));
        }
    }
}
