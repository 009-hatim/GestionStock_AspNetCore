using System;

namespace Gestion.Models
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalEmployees { get; set; }
        public int LowStockProducts { get; set; }
        public int OutOfStockProducts { get; set; }

        // Optional: Add these for more detailed info
        public int ActiveSuppliers { get; set; }
        public int RecentStockMovements { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}