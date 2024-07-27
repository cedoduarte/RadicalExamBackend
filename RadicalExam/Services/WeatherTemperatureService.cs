using Newtonsoft.Json.Linq;

namespace RadicalExam.Services
{
    public interface IWeatherTemperatureService
    {
        string ApiKey { get; set; }
        Task<string> GetTemperature(string countryCode, string cityName);
    }

    public class WeatherTemperatureService : IWeatherTemperatureService
    {
        private const string BaseUrl = "http://api.openweathermap.org/data/2.5/weather";
        public string ApiKey { get; set; }

        public async Task<string> GetTemperature(string countryCode, string cityName)
        {
            using HttpClient client = new HttpClient();
            string requestUri = $"{BaseUrl}?q={cityName},{countryCode}&appid={ApiKey}&units=metric";
            HttpResponseMessage response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject weatherData = JObject.Parse(responseBody);
            string temperature = "No Data";
            if (weatherData["main"] is not null)
            {
                temperature = weatherData["main"]["temp"].ToString();
            }
            return temperature;
        }
    }
}
