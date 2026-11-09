using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }
        public TimeSpan FinishTime { get; set; }
        public int FinishingPosition { get; set; }

        // Foreign Keys
        public int EnrolmentId { get; set; }
        [ForeignKey("EnrolmentId")]
        public Enrolment? Enrolment { get; set; }
    }
}
