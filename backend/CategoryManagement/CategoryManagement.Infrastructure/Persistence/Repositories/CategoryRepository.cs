using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CategoryManagement.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllWithConditionsAsync()
        {
            return await _context.Categories
                .Include(c => c.Conditions)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Category> GetByIdWithConditionsAsync(int id)
        {
            return await _context.Categories
                .Include(c => c.Conditions)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public override async Task<Category> AddAsync(Category entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public override async Task UpdateAsync(Category entity)
        {
            var existingCategory = await _context.Categories
                .Include(c => c.Conditions)
                .FirstOrDefaultAsync(c => c.Id == entity.Id);

            if (existingCategory == null)
                return;

            // Update basic properties
            existingCategory.Name = entity.Name;
            existingCategory.Description = entity.Description;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            // Handle conditions - replace all conditions
            if (entity.Conditions != null)
            {
                // Remove all existing conditions
                _context.CategoryConditions.RemoveRange(existingCategory.Conditions);

                // Add new conditions
                foreach (var condition in entity.Conditions)
                {
                    condition.CategoryId = entity.Id;
                    condition.CreatedAt = DateTime.UtcNow;
                    condition.UpdatedAt = DateTime.UtcNow;
                }
                existingCategory.Conditions = entity.Conditions.ToList();
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveConditionsAsync(int categoryId, IEnumerable<int> conditionIds)
        {
            var category = await _context.Categories
                .Include(c => c.Conditions)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return;

            var conditionsToRemove = category.Conditions
                .Where(c => conditionIds.Contains(c.Id))
                .ToList();

            foreach (var condition in conditionsToRemove)
            {
                category.Conditions.Remove(condition);
            }

            await _context.SaveChangesAsync();
        }

        public override async Task DeleteAsync(Category entity)
        {
            // The conditions will be deleted automatically due to cascade delete
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
