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

        /// <summary>
        /// Initializes a new instance of the <see cref="StocskRepository"/> class.
        /// </summary>
        /// <param name="context">The database context to interact with the database.</param>
        /// <param name="logger">The logger instance for logging information.</param>
        public StocskRepository(ApplicationDbContext context, ILogger<StocskRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Creates a new buy order in the database.
        /// </summary>
        /// <param name="order">The buy order to be created.</param>
        /// <returns>The created buy order.</returns>
        public async Task<BuyOrder> CreateBuyOrder(BuyOrder order)
        {
            await _context.BuyOrders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        /// <summary>
        /// Creates a new sell order in the database.
        /// </summary>
        /// <param name="order">The sell order to be created.</param>
        /// <returns>The created sell order.</returns>
        public async Task<SellOrder> CreateSellOrder(SellOrder order)
        {
            await _context.SellOrders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        /// <summary>
        /// Retrieves all buy orders from the database, ordered by the date and time of the order in descending order.
        /// </summary>
        /// <returns>A list of buy orders, or null if no orders are found.</returns>
        public async Task<List<BuyOrder>?> GetBuyOrders()
        {
            return await _context.BuyOrders.OrderByDescending(b => b.DateAndTimeOfOrder).ToListAsync();
        }

        /// <summary>
        /// Retrieves all sell orders from the database, ordered by the date and time of the order in descending order.
        /// </summary>
        /// <returns>A list of sell orders, or null if no orders are found.</returns>
        public async Task<List<SellOrder>?> GetSellOrders()
        {
            return await _context.SellOrders.OrderByDescending(s => s.DateAndTimeOfOrder).ToListAsync();
        }
    }
}
