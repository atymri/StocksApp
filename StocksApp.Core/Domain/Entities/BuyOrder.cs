using System.ComponentModel.DataAnnotations;

namespace StocksApp.Entities
{
    public class BuyOrder
    {
        [Key]
        public Guid BuyOrderID { get; set; }
        
        public string StockSymbol { get; set; }
        
        [Required(ErrorMessage = "Stock Name cant be null or empty.")]
        public string StockName { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateAndTimeOfOrder { get; set; } = DateTime.UtcNow;
        
        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 1000.")]
        public uint Quantity { get; set; }
        
        [Range(2, 1000, ErrorMessage = "Price must be between 1 and 1000")]
        public double Price { get; set; }

    }
}
