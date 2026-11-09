using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using RaceDayManagement.Web.Models;

namespace RaceDayManagement.Web.Services
{
    public class RaceDayApiService : IRaceDayApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly JsonSerializerOptions _jsonOptions;

        public RaceDayApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient("RaceDayAPI");

        public async Task<List<EventViewModel>> GetEventsAsync()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/events");
            if (!response.IsSuccessStatusCode) return new List<EventViewModel>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EventViewModel>>(content, _jsonOptions) ?? new List<EventViewModel>();
        }

        public async Task<EventViewModel?> GetEventByIdAsync(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/events/{id}");
            if (!response.IsSuccessStatusCode) return null;

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<EventViewModel>(content, _jsonOptions);
        }

        public async Task<bool> CreateEventAsync(CreateEventViewModel model)
        {
            var client = CreateClient();
            using var formData = new MultipartFormDataContent();

            formData.Add(new StringContent(model.Title ?? ""), nameof(model.Title));
            formData.Add(new StringContent(model.Description ?? ""), nameof(model.Description));
            formData.Add(new StringContent(model.EventDate.ToString("o")), nameof(model.EventDate));
            formData.Add(new StringContent(model.Location ?? ""), nameof(model.Location));
            formData.Add(new StringContent(model.Distance ?? ""), nameof(model.Distance));
            formData.Add(new StringContent(model.EventType ?? ""), nameof(model.EventType));
            formData.Add(new StringContent(model.EntryFee.ToString()), nameof(model.EntryFee));

            if (model.ImageFile != null)
            {
                var streamContent = new StreamContent(model.ImageFile.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.ImageFile.ContentType);
                formData.Add(streamContent, "ImageFile", model.ImageFile.FileName);
            }

            var response = await client.PostAsync("api/events", formData);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventAsync(EventViewModel model)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/events/{model.EventId}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/events/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddCategoryAsync(int eventId, string categoryName)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(new { EventId = eventId, CategoryName = categoryName });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/categories", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EnrollParticipantAsync(int eventId, int participantId, string category)
        {
            var client = CreateClient();

            var enrolmentPayload = new
            {
                EventId = eventId,
                ParticipantId = participantId,
                CategoryId = category
            };

            var json = JsonSerializer.Serialize(enrolmentPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/enrolments", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<EnrolmentViewModel>> GetEventEnrolmentsAsync(int eventId)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/enrolments/event/{eventId}");
            if (!response.IsSuccessStatusCode) return new List<EnrolmentViewModel>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EnrolmentViewModel>>(content, _jsonOptions) ?? new List<EnrolmentViewModel>();
        }

        public async Task<List<EnrolmentViewModel>> GetParticipantEnrolmentsAsync(int participantId)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/enrolments/participant/{participantId}");
            if (!response.IsSuccessStatusCode) return new List<EnrolmentViewModel>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<EnrolmentViewModel>>(content, _jsonOptions) ?? new List<EnrolmentViewModel>();
        }

        public async Task<bool> SubmitResultAsync(int enrolmentId, string finishTime, int position)
        {
            var client = CreateClient();
            var json = JsonSerializer.Serialize(new { EnrolmentId = enrolmentId, FinishTime = finishTime, Position = position });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/results", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<ParticipantResultViewModel>> GetParticipantResultsAsync(int participantId)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/results/participant/{participantId}");
            if (!response.IsSuccessStatusCode) return new List<ParticipantResultViewModel>();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ParticipantResultViewModel>>(content, _jsonOptions) ?? new List<ParticipantResultViewModel>();
        }
    }
}
