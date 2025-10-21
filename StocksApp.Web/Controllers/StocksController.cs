using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Web.Models;
using System.Security.Cryptography.Xml;

namespace StocksApp.Web.Controllers
{
    /// <summary>  
    /// Controller responsible for handling stock-related operations.  
    /// </summary>  
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly ILogger<StocksController> _logger;

        /// <summary>  
        /// Initializes a new instance of the <see cref="StocksController"/> class.  
        /// </summary>  
        /// <param name="finnhubService">Service for interacting with the Finnhub API.</param>  
        /// <param name="tradingOptions">Configuration options for trading.</param>  
        /// <param name="logger">Logger instance for logging operations.</param>  
        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, ILogger<StocksController> logger)
        {
            _logger = logger;
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
        }

        /// <summary>  
        /// Displays a list of stocks, optionally filtered by a specific stock symbol or popular stocks.  
        /// </summary>  
        /// <param name="stock">The stock symbol to explore (optional).</param>  
        /// <param name="showAll">Indicates whether to show all stocks or only the top 25 popular stocks.</param>  
        /// <returns>A view displaying the list of stocks.</returns>  
        [Route("[action]/{stock?}")]
        [Route("~/[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            // Get company profile from API server  
            List<Dictionary<string, string>>? stocksDictionary = await _finnhubService.GetStocks();

            List<Stock> stocks = new List<Stock>();

            if (stocksDictionary is not null)
            {
                // Filter stocks  
                if (!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? Top25PopularStocksList = _tradingOptions.Top25PopularStocks.Split(",");
                    if (Top25PopularStocksList is not null)
                    {
                        stocksDictionary = stocksDictionary
                         .Where(temp => Top25PopularStocksList.Contains(Convert.ToString(temp["symbol"])))
                         .ToList();
                    }
                }

                stocks = stocksDictionary
                 .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                .ToList();
            }

            ViewBag.stock = stock;
            return View(stocks);
        }
    }
}
