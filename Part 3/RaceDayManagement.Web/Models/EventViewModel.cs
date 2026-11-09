namespace RaceDayManagement.Web.Models
{
    public class EventViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public double Distance { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? BannerUrl { get; set; }
        public decimal EntryFee { get; set; }

        // Fallback image helper property
        public string DisplayImageUrl => string.IsNullOrWhiteSpace(ImageUrl)
            ? "/images/cape-peninsula.webp"
            : ImageUrl;
    }
}
