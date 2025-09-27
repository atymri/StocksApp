using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Web.Models;
using System.Security.Cryptography.Xml;

namespace StocksApp.Web.Controllers
{
    [Route("[controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradingOptions _tradingOptions;
        private readonly ILogger<StocksController> _logger;
        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions, ILogger<StocksController> logger)
        {
            _logger = logger;
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions.Value;
        }

        [Route("[action]")]
        [Route("[action]/{stock?}")]
        public async Task<IActionResult> Explore(string? stock, bool showAll = false)
        {
            var stockDict = await _finnhubService.GetStocks();
            List<Stock> stocks = new List<Stock>();

            if(stockDict is not null)
            {
                if(!showAll && _tradingOptions.Top25PopularStocks != null)
                {
                    string[]? top25 = _tradingOptions.Top25PopularStocks.Split(',');
                    if(top25 is not null)
                    {
                        stockDict = stockDict.Where(temp => top25.Contains(Convert.ToString(temp["symbol"])))
                            .ToList();
                    }
                }

                stocks = stockDict
                   .Select(temp => new Stock() { StockName = Convert.ToString(temp["description"]), StockSymbol = Convert.ToString(temp["symbol"]) })
                   .ToList();
            }
            ViewBag.Stock = stock;
            return View(stocks);
        }
    }
}
