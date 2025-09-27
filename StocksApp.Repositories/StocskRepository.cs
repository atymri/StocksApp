using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StocksApp.Entities;
using StocksApp.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Repositories
{
    public class StocskRepository : IStocksRepository
    {
        private readonly ILogger<StocskRepository> _logger;
        private readonly ApplicationDbContext _context;
        public StocskRepository(ApplicationDbContext context, ILogger<StocskRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder order)
        {
            await _context.BuyOrders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<SellOrder> CreateSellOrder(SellOrder order)
        {
            await _context.SellOrders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<BuyOrder>?> GetBuyOrders()
        {
            return await _context.BuyOrders.OrderByDescending(b => b.DateAndTimeOfOrder).ToListAsync();
        }

        public async Task<List<SellOrder>?> GetSellOrders()
        {
            return await _context.SellOrders.OrderByDescending(s => s.DateAndTimeOfOrder).ToListAsync();
        }
    }
}
