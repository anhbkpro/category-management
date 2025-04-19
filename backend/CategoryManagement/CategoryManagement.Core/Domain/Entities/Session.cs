namespace CategoryManagement.Core.Domain.Entities
{
    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public bool IsOnline { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<SessionTag> SessionTags { get; set; }
        public ICollection<SessionSpeaker> SessionSpeakers { get; set; }
    }
}
