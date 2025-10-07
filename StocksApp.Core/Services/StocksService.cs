using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;
using StocksApp.Services.Helpers;

namespace StocksApp.Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepo;
        public StocksService(IStocksRepository stocksRepo)
        {
            _stocksRepo = stocksRepo;
        }
        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var buyOrder = request.ToBuyOrder();
            buyOrder.BuyOrderID = Guid.NewGuid();
            
            buyOrder = await _stocksRepo.CreateBuyOrder(buyOrder);
            
            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            ValidationHelper.ModelValidation(request);

            var sellOrder = request.ToSellOrder();
            sellOrder.SellOrderID = Guid.NewGuid();

            sellOrder = await _stocksRepo.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var orders = await _stocksRepo.GetBuyOrders();
            return orders.Select(order => order.ToBuyOrderResponse()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders() 
        {
            var orders = await _stocksRepo.GetSellOrders();
            return orders.Select(order => order.ToSellOrderResponse()).ToList();
        }
    }
}
