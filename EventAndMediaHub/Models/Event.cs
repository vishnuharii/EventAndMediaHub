using System.ComponentModel.DataAnnotations;

namespace EventAndMediaHub.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public virtual Location Locations { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set;}
        public int UserId { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<User> Users { get; set; }

    }
    public class EventDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
