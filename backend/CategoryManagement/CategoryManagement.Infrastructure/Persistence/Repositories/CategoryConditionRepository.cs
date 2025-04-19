using System.Collections.Generic;
using System.Threading.Tasks;
using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain;
using CategoryManagement.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CategoryManagement.Infrastructure.Persistence.Repositories
{
    public class CategoryConditionRepository : Repository<CategoryCondition>, ICategoryConditionRepository
    {
        public CategoryConditionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CategoryCondition>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.CategoryConditions
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}
