using System.Collections.Generic;

namespace Gestion.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PurchasePrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Quantity { get; set; }
        public int MinimumStock { get; set; }

        // Foreign keys - make them required (non-nullable)
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}