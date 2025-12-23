using System;

namespace Gestion.Models
{
    public class StockMovement
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public string Type { get; set; } // "IN" or "OUT"
        public int Quantity { get; set; }
    }
}
