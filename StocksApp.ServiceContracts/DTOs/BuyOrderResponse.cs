using ServiceContracts.DTO;
using StocksApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts.DTOs
{
    public class BuyOrderResponse : IOrderResponse
    {
        public Guid BuyOrderID { get; set; }

        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public OrderType TypeOfOrder => OrderType.BuyOrder;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(BuyOrderResponse)) return false;

            var other = (BuyOrderResponse)obj;
            return this.BuyOrderID == other.BuyOrderID &&
                   this.StockSymbol == other.StockSymbol &&
                   this.StockName == other.StockName &&
                   this.DateAndTimeOfOrder == other.DateAndTimeOfOrder &&
                   this.Quantity == other.Quantity &&
                   this.Price == other.Price &&
                   this.TradeAmount == other.TradeAmount;
        }

        public override int GetHashCode()
        {
            return StockSymbol.GetHashCode();
        }

        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderID}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Buy Price: {Price}, Trade Amount: {TradeAmount}";
        }
    }

    public static class BuyOrderExtension
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrder buyOrder)
        {
            if (buyOrder == null) return null;
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrder.BuyOrderID,
                StockSymbol = buyOrder.StockSymbol,
                StockName = buyOrder.StockName,
                Price = buyOrder.Price,
                DateAndTimeOfOrder = buyOrder.DateAndTimeOfOrder,
                Quantity = buyOrder.Quantity,
                TradeAmount = buyOrder.Price * buyOrder.Quantity
            };
        }

    }
}
