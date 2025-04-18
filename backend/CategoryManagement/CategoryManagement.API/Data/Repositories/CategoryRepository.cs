using Microsoft.EntityFrameworkCore;

public interface ICategoryRepository : IRepository<Category>
{
    Task<IEnumerable<Category>> GetAllWithConditionsAsync();
    Task<Category> GetByIdWithConditionsAsync(int id);
    Task RemoveConditionsAsync(int categoryId, IEnumerable<int> conditionIds);
}

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
        // Ensure CreatedAt is set
        if (entity.CreatedAt == default)
        {
            entity.CreatedAt = DateTime.UtcNow;
        }

        return await base.AddAsync(entity);
    }

    public override async Task UpdateAsync(Category entity)
    {
        // Set UpdatedAt timestamp
        entity.UpdatedAt = DateTime.UtcNow;

        // Get existing entity from db
        var existingCategory = await _context.Categories
            .Include(c => c.Conditions)
            .FirstOrDefaultAsync(c => c.Id == entity.Id);

        if (existingCategory == null)
        {
            throw new KeyNotFoundException($"Category with ID {entity.Id} not found");
        }

        // Update the properties of the existing entity
        _context.Entry(existingCategory).CurrentValues.SetValues(entity);

        // Handle conditions - remove, update, add
        // Remove conditions that are not in the updated entity
        var conditionIdsToKeep = entity.Conditions.Select(c => c.Id).Where(id => id != 0).ToList();
        var conditionsToRemove = existingCategory.Conditions
            .Where(c => !conditionIdsToKeep.Contains(c.Id))
            .ToList();

        foreach (var conditionToRemove in conditionsToRemove)
        {
            _context.CategoryConditions.Remove(conditionToRemove);
        }

        // Update existing and add new conditions
        foreach (var condition in entity.Conditions)
        {
            if (condition.Id == 0)
            {
                // New condition
                condition.CategoryId = entity.Id;
                existingCategory.Conditions.Add(condition);
            }
            else
            {
                // Existing condition - update it
                var existingCondition = existingCategory.Conditions
                    .FirstOrDefault(c => c.Id == condition.Id);

                if (existingCondition != null)
                {
                    _context.Entry(existingCondition).CurrentValues.SetValues(condition);
                }
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task RemoveConditionsAsync(int categoryId, IEnumerable<int> conditionIds)
    {
        var conditionsToRemove = await _context.CategoryConditions
            .Where(c => c.CategoryId == categoryId && conditionIds.Contains(c.Id))
            .ToListAsync();

        if (conditionsToRemove.Any())
        {
            _context.CategoryConditions.RemoveRange(conditionsToRemove);
            await _context.SaveChangesAsync();
        }
    }

    public override async Task DeleteAsync(Category entity)
    {
        // This will cascade delete all conditions due to the foreign key relationship
        await base.DeleteAsync(entity);
    }
}
