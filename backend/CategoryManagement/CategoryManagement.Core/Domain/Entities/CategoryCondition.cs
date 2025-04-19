using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CategoryManagement.Core.Domain.Entities;

public class CategoryCondition
{
    public int Id { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public ConditionType Type { get; set; }

    [Required]
    [MaxLength(500)]
    public string Value { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey(nameof(CategoryId))]
    public Category? Category { get; set; }
}

public enum ConditionType
{
    IncludeTag,
    ExcludeTag,
    Location,
    StartDateMin,
    StartDateMax
}
