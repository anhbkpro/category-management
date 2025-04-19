using System.ComponentModel.DataAnnotations;

namespace CategoryManagement.Core.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<CategoryCondition> Conditions { get; set; } = new List<CategoryCondition>();
}
