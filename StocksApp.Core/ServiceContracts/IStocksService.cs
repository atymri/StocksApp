using StocksApp.ServiceContracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts
{
    public interface IStocksService
    {
        Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest request);
        Task<SellOrderResponse> CreateSellOrder(SellOrderRequest request);
        Task<List<BuyOrderResponse>> GetBuyOrders();
        Task<List<SellOrderResponse>> GetSellOrders();
    }
}
