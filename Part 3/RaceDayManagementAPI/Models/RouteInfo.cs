using System.ComponentModel.DataAnnotations;

namespace RaceDayManagementAPI.Models
{
    public class RouteInfo
    {
        [Key]
        public int RouteId { get; set; }

        [Required]
        [StringLength(100)]
        public string StartPoint { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EndPoint { get; set; } = string.Empty;
        public string TerrainType { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Event> Events { get; set; } = new List<Event>();

    }
}
