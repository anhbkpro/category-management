using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CategoryManagement.Infrastructure.Persistence.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        public SessionRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PagedResult<Session>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending)
        {
            // Get the category with conditions
            var category = await _context.Categories
                .Include(c => c.Conditions)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return new PagedResult<Session> { Items = new List<Session>(), TotalCount = 0 };

            // Start with all sessions
            var query = _context.Sessions
                .Include(s => s.SessionTags)
                    .ThenInclude(st => st.Tag)
                .Include(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
                .AsQueryable();

            // Apply category conditions
            foreach (var condition in category.Conditions)
            {
                switch (condition.Type)
                {
                    case ConditionType.IncludeTag:
                        query = query.Where(s => s.SessionTags.Any(st => st.Tag.Name == condition.Value));
                        break;
                    case ConditionType.ExcludeTag:
                        query = query.Where(s => !s.SessionTags.Any(st => st.Tag.Name == condition.Value));
                        break;
                    case ConditionType.Location:
                        query = query.Where(s => s.Location == condition.Value);
                        break;
                    case ConditionType.StartDateMin:
                        if (DateTime.TryParse(condition.Value, out DateTime minDate))
                        {
                            query = query.Where(s => s.StartDate >= minDate);
                        }
                        break;
                    case ConditionType.StartDateMax:
                        if (DateTime.TryParse(condition.Value, out DateTime maxDate))
                        {
                            query = query.Where(s => s.StartDate <= maxDate);
                        }
                        break;
                }
            }

            // Apply sorting
            query = ApplySorting(query, sortBy, ascending);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Session>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        private IQueryable<Session> ApplySorting(IQueryable<Session> query, string sortBy, bool ascending)
        {
            switch (sortBy?.ToLower())
            {
                case "title":
                    return ascending ? query.OrderBy(s => s.Title) : query.OrderByDescending(s => s.Title);
                case "startdate":
                    return ascending ? query.OrderBy(s => s.StartDate) : query.OrderByDescending(s => s.StartDate);
                case "enddate":
                    return ascending ? query.OrderBy(s => s.EndDate) : query.OrderByDescending(s => s.EndDate);
                case "location":
                    return ascending ? query.OrderBy(s => s.Location) : query.OrderByDescending(s => s.Location);
                default:
                    return ascending ? query.OrderBy(s => s.StartDate) : query.OrderByDescending(s => s.StartDate);
            }
        }
    }
}
