using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using StocksApp.ServiceContracts;
using StocksApp.ServiceContracts.DTOs;
using StocksApp.Services.Helpers;

namespace StocksApp.Services
{
    /// <summary>  
    /// Service class for managing stock-related operations.  
    /// </summary>  
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepo;

        /// <summary>  
        /// Initializes a new instance of the <see cref="StocksService"/> class.  
        /// </summary>  
        /// <param name="stocksRepo">The repository for stock operations.</param>  
        public StocksService(IStocksRepository stocksRepo)
        {
            _stocksRepo = stocksRepo;
        }

        /// <summary>  
        /// Creates a new buy order.  
        /// </summary>  
        /// <param name="request">The buy order request containing order details.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the created buy order response.</returns>  
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>  
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

        /// <summary>  
        /// Creates a new sell order.  
        /// </summary>  
        /// <param name="request">The sell order request containing order details.</param>  
        /// <returns>A task that represents the asynchronous operation. The task result contains the created sell order response.</returns>  
        /// <exception cref="ArgumentNullException">Thrown when the request is null.</exception>  
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

        /// <summary>  
        /// Retrieves all buy orders.  
        /// </summary>  
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of buy order responses.</returns>  
        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            var orders = await _stocksRepo.GetBuyOrders();
            return orders.Select(order => order.ToBuyOrderResponse()).ToList();
        }

        /// <summary>  
        /// Retrieves all sell orders.  
        /// </summary>  
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of sell order responses.</returns>  
        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            var orders = await _stocksRepo.GetSellOrders();
            return orders.Select(order => order.ToSellOrderResponse()).ToList();
        }
    }
}
