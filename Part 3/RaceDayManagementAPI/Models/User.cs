using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // The Organiser or Participant
        [Required]
        [StringLength(100)]
        public string Role { get; set; } = "Participant";

        // Navigation Propertites
        public ICollection<Event>? OrganizedEvents { get; set; }
        public ICollection<Enrolment>? Enrolments { get; set; }

    }
}
