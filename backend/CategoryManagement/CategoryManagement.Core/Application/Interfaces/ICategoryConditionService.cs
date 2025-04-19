using CategoryManagement.Core.Application.DTOs;

namespace CategoryManagement.Core.Application.Interfaces
{
    public interface ICategoryConditionService
    {
        Task<IEnumerable<CategoryConditionDto>> GetConditionsByCategoryIdAsync(int categoryId);
        Task<CategoryConditionDto> GetCategoryConditionByIdAsync(int id);
        Task<CategoryConditionDto> CreateCategoryConditionAsync(CategoryConditionDto conditionDto);
        Task<CategoryConditionDto> UpdateCategoryConditionAsync(CategoryConditionDto conditionDto);
        Task DeleteCategoryConditionAsync(int id);
    }
}
