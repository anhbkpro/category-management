public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public ICollection<CategoryConditionDto> Conditions { get; set; } = new List<CategoryConditionDto>();
}
