using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Gestion.Models;
using Gestion.Helpers;  // Add this line
using System.Threading.Tasks;

namespace Gestion.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // REDIRECT BASED ON ROLE
        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("AdminDashboard");

            return RedirectToAction("EmployeeDashboard");
        }

        // ADMIN DASHBOARD
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            var dashboard = new DashboardViewModel
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalCategories = await _context.Categories.CountAsync(),
                TotalSuppliers = await _context.Suppliers.CountAsync(),
                TotalEmployees = await _context.Users.Where(u => u.Role == "Employee").CountAsync(),
                LowStockProducts = await _context.Products.CountAsync(p => p.Quantity <= p.MinimumStock && p.Quantity > 0),
                OutOfStockProducts = await _context.Products.CountAsync(p => p.Quantity == 0)
            };

            return View(dashboard);
        }

        // EMPLOYEE DASHBOARD
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EmployeeDashboard()
        {
            var dashboard = new DashboardViewModel
            {
                TotalProducts = await _context.Products.CountAsync(),
                TotalSuppliers = await _context.Suppliers.CountAsync(),
                LowStockProducts = await _context.Products.CountAsync(p => p.Quantity <= p.MinimumStock && p.Quantity > 0),
                OutOfStockProducts = await _context.Products.CountAsync(p => p.Quantity == 0),
                TotalCategories = await _context.Categories.CountAsync() // Employees can see categories count too
            };

            return View(dashboard);
        }

        // TEST HASH PAGE (for debugging password hashing)
        [Authorize(Roles = "Admin")]
        public IActionResult TestHash()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult TestHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Hash = "Veuillez entrer un mot de passe";
                return View();
            }

            var hash = PasswordHasher.HashPassword(password);  // Now this should work
            ViewBag.Hash = hash;
            ViewBag.OriginalPassword = password;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}