using System.Collections.Generic;

namespace Gestion.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation
        public List<Product> Products { get; set; } = new List<Product>();
    }
}