using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name cant be null or empty")]
        public string StockName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateAndTimeOfOrder { get; set; } = DateTime.UtcNow;

        [Range(1, 1000, ErrorMessage = "Quantity must be between 1 and 10000")]
        public uint Quantity { get; set; }

        [Range(1, 1000, ErrorMessage = "Price must be between 1 and 10000")]
        public double Price { get; set; }
    }
}
