using Microsoft.AspNetCore.Mvc;
using RaceDayManagement.Web.Models;
using System.Text;
using System.Text.Json;

namespace RaceDayManagement.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // Explicitly map to match the API's LoginDto expectations
            var apiPayload = new
            {
                email = model.Email,
                passwordHash = model.Password
            };

            var content = new StringContent(JsonSerializer.Serialize(apiPayload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                using var jsonDoc = JsonDocument.Parse(responseData);
                var root = jsonDoc.RootElement;

                // Safely extract properties with null coalescing to resolve warnings
                string email = (root.TryGetProperty("email", out var emailProp) ? emailProp.GetString() :
                               (root.TryGetProperty("Email", out emailProp) ? emailProp.GetString() : null)) ?? model.Email ?? string.Empty;

                string role = (root.TryGetProperty("role", out var roleProp) ? roleProp.GetString() :
                              (root.TryGetProperty("Role", out roleProp) ? roleProp.GetString() : null)) ?? "Participant";

                int userId = root.TryGetProperty("id", out var idProp) ? idProp.GetInt32() :
                             (root.TryGetProperty("Id", out idProp) ? idProp.GetInt32() : 0);

                // Save user details to session
                HttpContext.Session.SetString("UserEmail", email);
                HttpContext.Session.SetString("UserRole", role);
                HttpContext.Session.SetInt32("UserId", userId);

                // Redirect based on role
                if (role.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("Dashboard", "Organiser");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your credentials.");
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // Explicitly map properties to match the backend API RegisterDto expectations
            var apiPayload = new
            {
                name = model.Name,
                email = model.Email,
                passwordHash = model.Password,
                role = model.Role
            };

            var content = new StringContent(JsonSerializer.Serialize(apiPayload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction(nameof(Login));
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Registration failed. Email might already be in use or details are invalid.");
            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
