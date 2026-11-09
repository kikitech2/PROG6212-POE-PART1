using RaceDayManagement.Web.Models;

namespace RaceDayManagement.Web.Services
{
    public interface IRaceDayApiService
    {
        // Events & RouteInfo & Banner Uploads
        Task<List<EventViewModel>> GetEventsAsync();
        Task<EventViewModel?> GetEventByIdAsync(int id);
        Task<bool> CreateEventAsync(CreateEventViewModel model);
        Task<bool> UpdateEventAsync(EventViewModel model);
        Task<bool> DeleteEventAsync(int id);

        // Categories
        Task<bool> AddCategoryAsync(int eventId, string categoryName);

        // Enrolments
        Task<bool> EnrollParticipantAsync(int eventId, int participantId, string category);
        Task<List<EnrolmentViewModel>> GetEventEnrolmentsAsync(int eventId);
        Task<List<EnrolmentViewModel>> GetParticipantEnrolmentsAsync(int participantId);

        // Results
        Task<bool> SubmitResultAsync(int enrolmentId, string finishTime, int position);
        Task<List<ParticipantResultViewModel>> GetParticipantResultsAsync(int participantId);
    }
}

