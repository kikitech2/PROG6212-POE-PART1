using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementAPI.Models
{
    public class EventCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation Properities
        public ICollection<Event>? Events { get; set; }
        public ICollection<Enrolment>? Enrolments { get; set; }

    }
}
