using Microsoft.EntityFrameworkCore;

public interface ISessionRepository : IRepository<Session>
{
    Task<PagedResult<Session>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending);
}

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

        // Start with all sessions and apply filters
        IQueryable<Session> query = _context.Sessions
            .Include(s => s.SessionTags)
            .ThenInclude(st => st.Tag)
            .Include(s => s.SessionSpeakers)
            .ThenInclude(ss => ss.Speaker);

        // Apply include tag conditions
        var includeTags = category.Conditions
            .Where(c => c.Type == ConditionType.IncludeTag)
            .Select(c => c.Value)
            .ToList();

        if (includeTags.Any())
        {
            foreach (var tag in includeTags)
            {
                query = query.Where(s => s.SessionTags.Any(st => st.Tag.Name.ToLower() == tag.ToLower()));
            }
        }

        // Apply exclude tag conditions
        var excludeTags = category.Conditions
            .Where(c => c.Type == ConditionType.ExcludeTag)
            .Select(c => c.Value)
            .ToList();

        if (excludeTags.Any())
        {
            foreach (var tag in excludeTags)
            {
                query = query.Where(s => !s.SessionTags.Any(st => st.Tag.Name.ToLower() == tag.ToLower()));
            }
        }

        // Apply location filter
        var location = category.Conditions
            .FirstOrDefault(c => c.Type == ConditionType.Location)?.Value;

        if (!string.IsNullOrEmpty(location))
        {
            query = query.Where(s => s.Location.ToLower() == location.ToLower());
        }

        // Apply date range filter
        var startDateMin = category.Conditions
            .FirstOrDefault(c => c.Type == ConditionType.StartDateMin)?.Value;

        if (!string.IsNullOrEmpty(startDateMin) && DateTime.TryParse(startDateMin, out var minDate))
        {
            query = query.Where(s => s.StartDate >= minDate);
        }

        var startDateMax = category.Conditions
            .FirstOrDefault(c => c.Type == ConditionType.StartDateMax)?.Value;

        if (!string.IsNullOrEmpty(startDateMax) && DateTime.TryParse(startDateMax, out var maxDate))
        {
            query = query.Where(s => s.StartDate <= maxDate);
        }

        // Apply sorting
        query = ApplySorting(query, sortBy, ascending);

        // Get total count for pagination
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
            case "location":
                return ascending ? query.OrderBy(s => s.Location) : query.OrderByDescending(s => s.Location);
            case "startdate":
            default:
                return ascending ? query.OrderBy(s => s.StartDate) : query.OrderByDescending(s => s.StartDate);
        }
    }
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
