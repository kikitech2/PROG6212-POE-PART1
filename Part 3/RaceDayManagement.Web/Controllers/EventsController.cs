using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RaceDayManagement.Web.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace RaceDayManagement.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Events/Index
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");
            var response = await client.GetAsync("api/events");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<EventViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return View(events);
            }

            return View(new List<EventViewModel>());
        }

        // GET: /Events/Create
        public IActionResult Create()
        {
            return View(new CreateEventViewModel());
        }

        // POST: /Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string imageUrl = string.Empty;
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            // 1. Upload image through the API endpoint instead of talking to Azure directly
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                using var content = new MultipartFormDataContent();
                using var fileStream = model.ImageFile.OpenReadStream();
                using var streamContent = new StreamContent(fileStream);

                content.Add(streamContent, "file", model.ImageFile.FileName);

                var uploadResponse = await client.PostAsync("api/events/upload-image", content);
                if (uploadResponse.IsSuccessStatusCode)
                {
                    var uploadResult = await uploadResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    if (uploadResult != null && uploadResult.ContainsKey("url"))
                    {
                        imageUrl = uploadResult["url"];
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to upload image via API.");
                    return View(model);
                }
            }

            // 2. Map ViewModel properties and send event details to backend API
            var apiPayload = new
            {
                Name = model.Title,
                Description = model.Description,
                Date = model.EventDate,
                Location = model.Location,
                EntryFee = model.EntryFee,
                ImageUrl = imageUrl
            };

            var response = await client.PostAsJsonAsync("api/events", apiPayload);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, "Failed to create event on backend API.");
            return View(model);
        }
    }
}
