using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StocksApp.RepositoryContracts;
using System.Text.Json;

namespace StocksApp.Repositories
{
    public class FinnHubRepository : IFinnHubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FinnHubRepository> _logger;
        public FinnHubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<FinnHubRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };
            var response = await client.SendAsync(request);
            string responseBody = await new StreamReader(response.Content.ReadAsStream()).ReadToEndAsync();

            Dictionary<string, object>? responseDict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDict == null)
                throw new InvalidOperationException("No resonse from server");

            if (responseDict.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDict["error"]));

            return responseDict;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpRequestMessage request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };
            var response = await client.SendAsync(request);
            string responseBody = await new StreamReader(response.Content.ReadAsStream()).ReadToEndAsync();

            Dictionary<string, object>? responseDict = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

            if (responseDict == null)
                throw new InvalidOperationException("No response from server");

            if (responseDict.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDict["error"]));

            return responseDict;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}")
            };

            var response = await client.SendAsync(request);
            string body = await response.Content.ReadAsStringAsync();

            var responseDict = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(body);

            if (responseDict == null)
                throw new InvalidOperationException("No response from server");

            return  responseDict;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbol)
        {
            var client = _httpClientFactory.CreateClient();
            var request= new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/search?q={stockSymbol}&token={_configuration["FinnhubToken"]}")
            };

            var response = await client.SendAsync(request);
            var body = await response.Content.ReadAsStringAsync();

            var responseDict = JsonSerializer.Deserialize<Dictionary<string, object>>(body);

            if (responseDict == null)
                throw new InvalidOperationException("No response from server");

            if (responseDict.ContainsKey("error"))
                throw new InvalidOperationException(Convert.ToString(responseDict["error"]));

            return responseDict;

        }
    }
}
