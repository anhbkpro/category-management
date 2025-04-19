namespace CategoryManagement.Core.Application.DTOs
{
    public class SessionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }
        public bool IsOnline { get; set; }
        public ICollection<string> Tags { get; set; }
        public ICollection<SpeakerDto> Speakers { get; set; }
    }
}
