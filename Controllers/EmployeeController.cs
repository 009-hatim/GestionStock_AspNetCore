using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Gestion.Helpers;
using Gestion.Models;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Admin")]   // Only admin can manage employees
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        // ===========================
        // INDEX
        // ===========================
        public async Task<IActionResult> Index()
        {
            var employees = await _context.Users
                .Where(u => u.Role == "Employee")
                .ToListAsync();

            return View(employees);
        }

        // ===========================
        // DETAILS
        // ===========================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Users.FindAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // ===========================
        // CREATE (GET)
        // ===========================
        public IActionResult Create()
        {
            return View();
        }

        // ===========================
        // CREATE (POST)
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User employee, string password)
        {
            // Clear modelstate to avoid incorrect client validation
            ModelState.Clear();

            employee.Role = "Employee";

            // --- Manual validation ---
            if (string.IsNullOrWhiteSpace(employee.Username))
                ModelState.AddModelError("Username", "Le nom d'utilisateur est requis.");

            if (string.IsNullOrWhiteSpace(employee.Email))
                ModelState.AddModelError("Email", "L'email est requis.");

            if (string.IsNullOrWhiteSpace(password))
                ModelState.AddModelError("password", "Le mot de passe est requis.");

            if (!ModelState.IsValid)
                return View(employee);

            // Duplicate username
            if (await _context.Users.AnyAsync(u => u.Username == employee.Username))
            {
                ModelState.AddModelError("Username", "Nom d'utilisateur déjà utilisé.");
                return View(employee);
            }

            // Duplicate email
            if (await _context.Users.AnyAsync(u => u.Email == employee.Email))
            {
                ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                return View(employee);
            }

            // Hash password
            employee.PasswordHash = PasswordHasher.HashPassword(password);
            employee.CreatedAt = DateTime.Now;

            _context.Add(employee);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Employé créé avec succès!";
            return RedirectToAction(nameof(Index));
        }

        // ===========================
        // EDIT (GET)
        // ===========================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Users.FindAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // ===========================
        // EDIT (POST) — FIXED VERSION
        // ===========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, User formEmployee)
        {
            if (id != formEmployee.Id)
                return NotFound();

            // Load original record
            var dbEmployee = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (dbEmployee == null)
                return NotFound();

            // Update only allowed fields
            dbEmployee.Username = formEmployee.Username;
            dbEmployee.Email = formEmployee.Email;

            // Force Role to remain Employee
            dbEmployee.Role = "Employee";

            // SAVE WITHOUT USING ModelState (because model contains required fields not present in form)
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Employé modifié avec succès !";
            return RedirectToAction(nameof(Index));
        }


        // ===========================
        // DELETE (GET)
        // ===========================
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _context.Users.FindAsync(id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // ===========================
        // DELETE (POST) — FIXED VERSION
        // ===========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (employee == null)
                return NotFound(); // Prevent concurrency error

            _context.Users.Remove(employee);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
