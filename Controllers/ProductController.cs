using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Gestion.Controllers
{

    [Authorize(Roles = "Admin,Employee")]
    public class ProductController : Controller
    { 


        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // GET: Product/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            ViewBag.SupplierId = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name");

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            // Debug: afficher toutes les erreurs
            if (!ModelState.IsValid)
            {
                foreach (var err in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("VALIDATION ERROR: " + err.ErrorMessage);
                }

                // Recharger dropdowns
                ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
                ViewBag.SupplierId = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name", product.SupplierId);

                return View(product);
            }

            // Ajout du produit
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Produit ajouté avec succès !";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR SAVING PRODUCT: " + ex.Message);
                ModelState.AddModelError("", "Impossible d'ajouter le produit.");

                // Recharger dropdowns
                ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
                ViewBag.SupplierId = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name", product.SupplierId);

                return View(product);
            }
        }
        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return NotFound();

            // Charger les listes déroulantes
            ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            ViewBag.SupplierId = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name", product.SupplierId);

            return View(product);
        }


        // POST: Product/Edit/5
        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product formProduct)
        {
            if (id != formProduct.Id)
                return NotFound();

            var dbProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (dbProduct == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", formProduct.CategoryId);
                ViewBag.SupplierId = new SelectList(await _context.Suppliers.ToListAsync(), "Id", "Name", formProduct.SupplierId);
                return View(formProduct);
            }

            // Mise à jour sécurisée des champs autorisés
            dbProduct.Name = formProduct.Name;
            dbProduct.PurchasePrice = formProduct.PurchasePrice;
            dbProduct.SalePrice = formProduct.SalePrice;
            dbProduct.Quantity = formProduct.Quantity;
            dbProduct.MinimumStock = formProduct.MinimumStock;
            dbProduct.CategoryId = formProduct.CategoryId;
            dbProduct.SupplierId = formProduct.SupplierId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == formProduct.Id))
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }



        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
                _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> OutOfStock()
        {
            var products = await _context.Products
                .Where(p => p.Quantity == 0)
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);
        }

        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> LowStock()
        {
            var products = await _context.Products
                .Where(p => p.Quantity <= p.MinimumStock && p.Quantity > 0)
                .ToListAsync();

            return View(products);
        }

    }

}
