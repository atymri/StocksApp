using StocksApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace StocksApp.ServiceContracts.DTOs
{
    public class SellOrderRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Stock symbol cant be null or empty")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name cant be null or empty")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 10000")]
        public uint Quantity { get; set; }

        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price { get; set; }

        public SellOrder ToSellOrder()
        {
            return new SellOrder() { StockSymbol = StockSymbol, StockName = StockName, Price = Price, DateAndTimeOfOrder = DateAndTimeOfOrder, Quantity = Quantity };
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (DateAndTimeOfOrder < new DateTime(2000, 1, 1))
            {
                results.Add(new ValidationResult(
                    "Date of the order should not be less than Jan 01, 2000."
                ));
            }

            return results;
        }
    }
}
