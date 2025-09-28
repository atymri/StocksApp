using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;
using StocksApp.Web.Filters.ActionFilters;
using StocksApp.Web.Models;
using System.Globalization;

namespace StocksApp.Web.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IFinnhubService _finnhubService;
        private readonly IStocksService _stockService; 
        private readonly IConfiguration _configuration;
        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IStocksService stockService,IConfiguration configuration)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _stockService = stockService;
            _configuration = configuration;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("[action]/{symbol?}")]
        public async Task<IActionResult> Index(string? symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                symbol = _tradingOptions.DefaultStockSymbol;

            var companyProfileDictionary = await _finnhubService.GetCompanyProfile(symbol);
            var stockPriceQuoteDictionary = await _finnhubService.GetStockPriceQuote(symbol);

            var stockTrade = new StockTrade() { StockSymbol = symbol };

            if (companyProfileDictionary != null && stockPriceQuoteDictionary != null)
            {
                stockTrade = new StockTrade()
                {
                    StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]),
                    StockName = Convert.ToString(companyProfileDictionary["name"]),
                    Price = double.Parse(stockPriceQuoteDictionary["c"].ToString(), CultureInfo.InvariantCulture)
                };
            }

            ViewBag.FinnhubToken = _configuration["FinnhubToken"];

            return View(stockTrade);
        }


        [HttpPost]
        [Route("[action]")]
        [CreateOrderActionFactorory]
        public IActionResult SellOrder(SellOrderRequest request)
        {
            var response = _stockService.CreateSellOrder(request);
            return RedirectToAction(nameof(Orders));
        }

        [HttpPost]
        [Route("[action]")]
        [CreateOrderActionFactorory]
        public IActionResult BuyOrder(BuyOrderRequest request)
        {
            var response = _stockService.CreateBuyOrder(request);
            return RedirectToAction(nameof(Orders));
        }


        [Route("[action]")]
        public async Task<IActionResult> Orders()
        {
            List<BuyOrderResponse> buyOrders = await _stockService.GetBuyOrders();
            List<SellOrderResponse> sellOrders = await _stockService.GetSellOrders();

            var orders = new Orders()
            {
                BuyOrders = buyOrders,
                SellOrders = sellOrders
            };

            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            var orders = new List<IOrderResponse>();
            var buyOrders = await _stockService.GetBuyOrders();
            var sellOrders = await _stockService.GetSellOrders();

            orders.AddRange(buyOrders);
            orders = orders.OrderByDescending(o => o.DateAndTimeOfOrder).ToList();

            ViewBag.TradingOptions = _tradingOptions;

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Bottom = 20, Left = 20, Right = 20 }
            };


        }
    }
}
