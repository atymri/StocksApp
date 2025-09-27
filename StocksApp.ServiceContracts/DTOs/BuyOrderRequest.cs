using ServiceContracts.DTO;
using StocksApp.Entities;
using System.ComponentModel.DataAnnotations;

namespace StocksApp.ServiceContracts.DTOs
{
    public class BuyOrderRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Stock symbol is required.")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "StockName is required")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000")]
        public uint Quantity { get; set; }

        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 1000")]
        public double Price { get; set; }

        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Quantity = Quantity,
                Price = Price,
                DateAndTimeOfOrder = DateAndTimeOfOrder
            };
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
