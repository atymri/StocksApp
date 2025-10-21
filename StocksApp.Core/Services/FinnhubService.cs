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

        /// <summary>
        /// Initializes a new instance of the <see cref="FinnhubServic"/> class.
        /// </summary>
        /// <param name="finnHubRepo">The repository for interacting with Finnhub API.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public FinnhubServic(IFinnHubRepository finnHubRepo, ILogger<FinnhubServic> logger)
        {
            _finnHubRepo = finnHubRepo;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the company profile for a given stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol to retrieve the company profile for.</param>
        /// <returns>A dictionary containing the company profile data.</returns>
        public async Task<Dictionary<string, object>> GetCompanyProfile(string stockSymbol)
        {
            var response = await _finnHubRepo.GetCompanyProfile(stockSymbol);
            return response;
        }

        /// <summary>
        /// Retrieves the stock price quote for a given stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol to retrieve the stock price quote for.</param>
        /// <returns>A dictionary containing the stock price quote data.</returns>
        public async Task<Dictionary<string, object>> GetStockPriceQuote(string stockSymbol)
        {
            var response = await _finnHubRepo.GetStockPriceQuote(stockSymbol);
            return response;
        }

        /// <summary>
        /// Retrieves a list of available stocks.
        /// </summary>
        /// <returns>A list of dictionaries, each containing stock information.</returns>
        public Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var response = _finnHubRepo.GetStocks();
            return response;
        }

        /// <summary>
        /// Searches for a stock based on the given stock symbol.
        /// </summary>
        /// <param name="stockSymbol">The stock symbol to search for.</param>
        /// <returns>A dictionary containing the search results, or null if no results are found.</returns>
        public Task<Dictionary<string, object>?> SearchStock(string stockSymbol)
        {
            var response = _finnHubRepo.SearchStocks(stockSymbol);
            return response;
        }
    }
}
