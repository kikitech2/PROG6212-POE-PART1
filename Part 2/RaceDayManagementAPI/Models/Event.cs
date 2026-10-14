using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayManagementAPI.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date {  get; set; }

        [Required]
        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        [Required]
        public double Distance { get; set; }

        [Required]
        [StringLength(100)]
        public string EventType { get; set; } = string.Empty;

        // Foreign Keys
        public int OrganiserId { get; set; }
        [ForeignKey("OrganiserId")]
        public User? Organiser { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public EventCategory? Category { get; set; }
        public int RouteId { get; set; }
        [ForeignKey("RouteId")]
        public RouteInfo? Route { get; set; }

        public ICollection<Enrolment>? Enrolments { get; set; }


    }
}
