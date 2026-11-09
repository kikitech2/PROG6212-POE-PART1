using Microsoft.AspNetCore.Mvc;
using RaceDayManagement.Web.Models;
using RaceDayManagement.Web.Services;

namespace RaceDayManagement.Web.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly IRaceDayApiService _apiService;

        public ParticipantController(IRaceDayApiService apiService)
        {
            _apiService = apiService;
        }

        // Helper check to ensure user is logged in as a Participant
        private IActionResult? ValidateParticipant()
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (string.IsNullOrEmpty(role) || !role.Equals("Participant", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Login", "Account");
            }
            return null;
        }

        // GET: /Participant/Index (Browse Public Events)
        public async Task<IActionResult> Index()
        {
            var authCheck = ValidateParticipant();
            if (authCheck != null) return authCheck;

            var events = await _apiService.GetEventsAsync();
            return View(events);
        }

        // GET: /Participant/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var authCheck = ValidateParticipant();
            if (authCheck != null) return authCheck;

            var eventItem = await _apiService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // POST: /Participant/Enroll
        [HttpPost]
        public async Task<IActionResult> Enroll(int eventId, string category)
        {
            var authCheck = ValidateParticipant();
            if (authCheck != null) return authCheck;

            // Retrieve current participant ID from session
            int participantId = HttpContext.Session.GetInt32("UserId") ?? 1;

            var success = await _apiService.EnrollParticipantAsync(eventId, participantId, category);
            if (success)
            {
                TempData["SuccessMessage"] = "Successfully enrolled in the event!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to enrol in the event.";
            }

            return RedirectToAction(nameof(MyEnrolments));
        }

        // GET: /Participant/MyEnrolments
        public async Task<IActionResult> MyEnrolments()
        {
            var authCheck = ValidateParticipant();
            if (authCheck != null) return authCheck;

            int participantId = HttpContext.Session.GetInt32("UserId") ?? 1;
            var enrolments = await _apiService.GetParticipantEnrolmentsAsync(participantId);
            return View(enrolments);
        }

        // GET: /Participant/MyResults
        public async Task<IActionResult> MyResults()
        {
            var authCheck = ValidateParticipant();
            if (authCheck != null) return authCheck;

            int participantId = HttpContext.Session.GetInt32("UserId") ?? 1;
            var results = await _apiService.GetParticipantResultsAsync(participantId);
            return View(results);
        }
    }
}
