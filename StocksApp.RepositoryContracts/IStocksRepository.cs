using StocksApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.RepositoryContracts
{
    public interface IStocksRepository
    {
        Task<BuyOrder> CreateBuyOrder(BuyOrder order);
        Task<SellOrder> CreateSellOrder(SellOrder order);
        Task<List<BuyOrder>?> GetBuyOrders();
        Task<List<SellOrder>?> GetSellOrders();
    }
}
