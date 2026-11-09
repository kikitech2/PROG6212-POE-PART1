namespace RaceDayManagement.Web.Models
{
    public class ParticipantResultViewModel
    {
        public int Id { get; set; }
        public int EnrolmentId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public string FinishTime { get; set; } = string.Empty;
        public int Position { get; set; }
    }
}
