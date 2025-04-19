namespace CategoryManagement.Core.Domain.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public ICollection<SessionTag> SessionTags { get; set; }
    }
}
