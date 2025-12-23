using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion.Models;
using Gestion.Helpers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Gestion.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine($"=== LOGIN ATTEMPT ===");
                Console.WriteLine($"Username: {model.Username}");
                Console.WriteLine($"Password: {model.Password}");

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == model.Username);

                if (user != null)
                {
                    Console.WriteLine($"User found: {user.Username}");
                    Console.WriteLine($"Stored hash: {user.PasswordHash}");
                    Console.WriteLine($"Stored hash length: {user.PasswordHash?.Length}");

                    // Generate hash from input password
                    var inputHash = PasswordHasher.HashPassword(model.Password ?? "");
                    Console.WriteLine($"Input hash: {inputHash}");
                    Console.WriteLine($"Input hash length: {inputHash.Length}");
                    Console.WriteLine($"Hashes match: {inputHash == user.PasswordHash}");

                    if (PasswordHasher.VerifyPassword(model.Password ?? "", user.PasswordHash ?? ""))
                    {
                        Console.WriteLine("Password verification SUCCESS");

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username ?? ""),
                            new Claim(ClaimTypes.Role, user.Role ?? ""),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RememberMe
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        Console.WriteLine("Login SUCCESS - Redirecting to Home");
                        if (user.Role == "Admin")
                            return RedirectToAction("AdminDashboard", "Home");

                        if (user.Role == "Employee")
                            return RedirectToAction("EmployeeDashboard", "Home");

                    }
                    else
                    {
                        Console.WriteLine("Password verification FAILED");
                    }
                }
                else
                {
                    Console.WriteLine("User NOT found in database");
                }

                ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
            }

            return View(model);
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, string password)
        {
            if (ModelState.IsValid)
            {
                // Check if username already exists
                if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Ce nom d'utilisateur existe déjà.");
                    return View(user);
                }

                user.PasswordHash = PasswordHasher.HashPassword(password);
                user.CreatedAt = DateTime.Now;

                _context.Add(user);
                await _context.SaveChangesAsync();

                // Auto-login after registration
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username ?? ""),
                    new Claim(ClaimTypes.Role, user.Role ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // POST: /Account/Logout - FIXED THIS METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}