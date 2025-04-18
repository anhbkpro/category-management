// Domain models
public class Session
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Location { get; set; }
    public bool IsOnline { get; set; }
    public virtual ICollection<SessionTag> SessionTags { get; set; }
    public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; }
}
