using Newtonsoft.Json.Linq;
using RadicalExam.Models;

namespace RadicalExam.Services
{
    public interface IBanxicoService
    {
        string Token { get; set; }
        Task<BanxicoSerie> GetExchangeRate(string startDate, string endDate);
    }

    public class BanxicoService : IBanxicoService
    {
        private const string BaseUrl = "https://www.banxico.org.mx/SieAPIRest/service/v1";
        public string Token { get; set; }

        public async Task<BanxicoSerie> GetExchangeRate(string startDate, string endDate)
        {
            using HttpClient client = new HttpClient();
            string requestUri = $"{BaseUrl}/series/SF63528/datos/{startDate}/{endDate}";
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            request.Headers.Add("Bmx-Token", Token);
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JObject bmxData = JObject.Parse(responseBody);
            var result = new BanxicoSerie();
            if (bmxData is null)
            {
                return result;
            }
            JArray serieArray = (JArray)bmxData["bmx"]["series"];
            result.IdSerie = serieArray[0]["idSerie"].ToString();
            result.Title = serieArray[0]["titulo"].ToString();
            var dataArray = (JArray)serieArray[0]["datos"];
            foreach (JObject recordObject in dataArray)
            {
                result.Records.Add(new BanxicoRecord()
                {
                    Date = recordObject["fecha"].ToString(),
                    Value = recordObject["dato"].ToString()
                });
            }
            return result;
        }
    }
}
