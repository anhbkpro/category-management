using System.Collections.Generic;
using System.Threading.Tasks;
using CategoryManagement.Core.Domain.Entities;

namespace CategoryManagement.Core.Application.Interfaces
{
    public interface ICategoryConditionRepository : IRepository<CategoryCondition>
    {
        Task<IEnumerable<CategoryCondition>> GetByCategoryIdAsync(int categoryId);
    }
}
