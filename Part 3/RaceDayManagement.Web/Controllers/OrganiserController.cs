using Microsoft.AspNetCore.Mvc;
using RaceDayManagement.Web.Models;
using System.Text.Json;

namespace RaceDayManagement.Web.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OrganiserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Organiser/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || !role.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var events = new List<EventViewModel>();

            try
            {
                var response = await client.GetAsync("api/events");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    events = JsonSerializer.Deserialize<List<EventViewModel>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<EventViewModel>();
                }
            }
            catch
            {
                events = new List<EventViewModel>();
            }

            return View(events);
        }

        // GET: /Organiser/CreateEvent
        [HttpGet]
        public IActionResult CreateEvent()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || !role.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        // POST: /Organiser/CreateEvent
        [HttpPost]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || !role.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // Clean up Distance string (e.g., "42.2km" -> 42.2) so it parses properly as a Double
            double parsedDistance = 0;
            if (!string.IsNullOrEmpty(model.Distance))
            {
                var numericString = new string(model.Distance.Where(c => char.IsDigit(c) || c == '.').ToArray());
                double.TryParse(numericString, out parsedDistance);
            }

            var apiPayload = new
            {
                Name = model.Title,
                Description = model.Description,
                Date = model.EventDate,
                Location = model.Location,
                Distance = parsedDistance,
                EventType = model.EventType,
                EntryFee = model.EntryFee
            };

            var content = new StringContent(JsonSerializer.Serialize(apiPayload), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/events", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction(nameof(Dashboard));
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"API Error: {errorContent}");
            return View(model);
        }

        // GET: /Organiser/Results?eventId=5
        [HttpGet]
        public IActionResult Results(int eventId)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || !role.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.EventId = eventId;
            var model = new ResultViewModel { EventId = eventId };
            return View(model);
        }

        // POST: /Organiser/Results
        [HttpPost]
        public async Task<IActionResult> Results(ResultViewModel model)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || !role.Equals("Organiser", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var content = new StringContent(JsonSerializer.Serialize(model), System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/results", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Participant result recorded successfully!";
                return RedirectToAction(nameof(Dashboard));
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"API Error: {errorContent}");
            return View(model);
        }
    }
}

