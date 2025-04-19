using CategoryManagement.Core.Domain.Entities;

namespace CategoryManagement.Core.Application.DTOs
{
    public class CategoryConditionDto
    {
        public int Id { get; set; }
        public ConditionType Type { get; set; }
        public string Value { get; set; }
    }
}
