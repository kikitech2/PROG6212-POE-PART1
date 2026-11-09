namespace RaceDayManagement.Web.Models
{
    public class ResultViewModel
    {
        public int ResultId { get; set; }
        public int EventId { get; set; }
        public int EnrolmentId { get; set; } // Replaces ParticipantName to satisfy FK constraint
        public string FinishTime { get; set; } = string.Empty; // e.g. "03:45:12"
        public int Position { get; set; }
    }
}