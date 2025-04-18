public class SessionTag
{
    public int SessionId { get; set; }
    public int TagId { get; set; }
    public virtual Session Session { get; set; }
    public virtual Tag Tag { get; set; }
}
