using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RaceDayManagement.Web.Models
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "Event title is required")]
        [Display(Name = "Event Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event date and time is required")]
        [Display(Name = "Event Date & Time")]
        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(7);

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Distance is required")]
        [Display(Name = "Distance")]
        public string Distance { get; set; } = string.Empty;

        [Required(ErrorMessage = "Event type is required")]
        [Display(Name = "Event Type")]
        public string EventType { get; set; } = string.Empty;

        [Display(Name = "Entry Fee (ZAR)")]
        [Range(0, 10000, ErrorMessage = "Fee must be a non-negative value")]
        public decimal EntryFee { get; set; }

        [Display(Name = "Event Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
