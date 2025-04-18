public class CategoryCondition
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public ConditionType Type { get; set; }
    public string Value { get; set; }
    public virtual Category Category { get; set; }
}


public enum ConditionType
{
    IncludeTag,
    ExcludeTag,
    Location,
    StartDateMin,
    StartDateMax
}
