using CategoryManagement.Core.Domain.Entities;

namespace CategoryManagement.Core.Application.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllWithConditionsAsync();
        Task<Category> GetByIdWithConditionsAsync(int id);
        Task RemoveConditionsAsync(int categoryId, IEnumerable<int> conditionIds);
    }
}
