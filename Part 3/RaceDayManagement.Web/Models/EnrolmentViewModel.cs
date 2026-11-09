namespace RaceDayManagement.Web.Models
{
    public class EnrolmentViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public int ParticipantId { get; set; }
        public string ParticipantName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public DateTime EnrolmentDate { get; set; }
    }
}
