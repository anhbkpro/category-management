public class Speaker
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Bio { get; set; }
    public string ProfileImage { get; set; }
    public virtual ICollection<SessionSpeaker> SessionSpeakers { get; set; }
}
