using ServiceContracts;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using StocksApp.RepositoryContracts;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class FinnhubServic : IFinnhubService
    {
        private readonly IFinnHubRepository _finnHubRepo;
        private readonly ILogger<IFinnhubService> _logger;
        public FinnhubServic(IFinnHubRepository finnHubRepo, ILogger<FinnhubServic> logger)
        {
            _finnHubRepo = finnHubRepo;
            _logger = logger;
        }
        
        public async Task<Dictionary<string, object>> GetCompanyProfile(string stockSymbol)
        {
           var response = await _finnHubRepo.GetCompanyProfile(stockSymbol);
           return response;
        }

        public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
        {
            var response = await _finnHubRepo.GetStockPriceQuote(stockSymbol);
            return response;
        }

        public Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var response = _finnHubRepo.GetStocks();
            return response;
        }

        public Task<Dictionary<string, object>?> SearchStock(string stockSymbol)
        {
            var response = _finnHubRepo.SearchStocks(stockSymbol);
            return response;
        }
    }
}
