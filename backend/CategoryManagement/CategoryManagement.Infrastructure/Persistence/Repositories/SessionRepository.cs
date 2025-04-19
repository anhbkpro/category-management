using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CategoryManagement.Infrastructure.Persistence.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly ILogger<SessionRepository> _logger;

        public SessionRepository(ApplicationDbContext context, ILogger<SessionRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public override async Task<IEnumerable<Session>> GetAllAsync()
        {
            return await _context.Sessions
                .Include(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
                .Include(s => s.SessionTags)
                    .ThenInclude(st => st.Tag)
                .ToListAsync();
        }

        public override async Task<Session> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(s => s.SessionSpeakers)
                    .ThenInclude(ss => ss.Speaker)
                .Include(s => s.SessionTags)
                    .ThenInclude(st => st.Tag)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public override async Task<Session> AddAsync(Session session)
        {
            _logger.LogInformation("Adding new session: {SessionTitle}", session.Title);
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public override async Task<Session> UpdateAsync(Session session)
        {
            _logger.LogInformation("Updating session: {SessionId} - {SessionTitle}", session.Id, session.Title);
            _context.Entry(session).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return session;
        }

        public override async Task DeleteAsync(Session session)
        {
            _logger.LogInformation("Deleting session: {SessionId} - {SessionTitle}", session.Id, session.Title);
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Session>> GetSessionsByCategoryAsync(int categoryId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CategoryId", categoryId),
                    new SqlParameter("@Page", 1),
                    new SqlParameter("@PageSize", int.MaxValue),
                    new SqlParameter("@SortBy", "StartDate"),
                    new SqlParameter("@Ascending", true)
                };

                var sql = "EXEC GetSessionsByCategory @CategoryId, @Page, @PageSize, @SortBy, @Ascending";

                var sessions = await _context.Sessions
                    .FromSqlRaw(sql, parameters)
                    .ToListAsync();

                // Load related data
                foreach (var session in sessions)
                {
                    await _context.Entry(session)
                        .Collection(s => s.SessionSpeakers)
                        .Query()
                        .Include(ss => ss.Speaker)
                        .LoadAsync();

                    await _context.Entry(session)
                        .Collection(s => s.SessionTags)
                        .Query()
                        .Include(st => st.Tag)
                        .LoadAsync();
                }

                _logger.LogInformation("Retrieved {Count} sessions for category {CategoryId}", sessions.Count, categoryId);
                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sessions for category {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<PagedResult<Session>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@CategoryId", categoryId),
                    new SqlParameter("@Page", page),
                    new SqlParameter("@PageSize", pageSize),
                    new SqlParameter("@SortBy", sortBy),
                    new SqlParameter("@Ascending", ascending),
                    new SqlParameter("@TotalCount", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                var sql = "EXEC GetSessionsByCategory @CategoryId, @Page, @PageSize, @SortBy, @Ascending, @TotalCount OUTPUT";

                // Execute the stored procedure and get the total count
                await _context.Database.ExecuteSqlRawAsync(sql, parameters);
                var totalCount = (int)parameters[5].Value;

                if (totalCount == 0)
                {
                    _logger.LogInformation("No sessions found for category {CategoryId}", categoryId);
                    return new PagedResult<Session>
                    {
                        Items = new List<Session>(),
                        TotalCount = 0,
                        CurrentPage = page,
                        PageSize = pageSize
                    };
                }

                // Get the sessions
                var sessions = await _context.Sessions
                    .FromSqlRaw("EXEC GetSessionsByCategory @CategoryId, @Page, @PageSize, @SortBy, @Ascending, @TotalCount OUTPUT",
                        parameters)
                    .ToListAsync();

                // Load related data
                foreach (var session in sessions)
                {
                    await _context.Entry(session)
                        .Collection(s => s.SessionSpeakers)
                        .Query()
                        .Include(ss => ss.Speaker)
                        .LoadAsync();

                    await _context.Entry(session)
                        .Collection(s => s.SessionTags)
                        .Query()
                        .Include(st => st.Tag)
                        .LoadAsync();
                }

                _logger.LogInformation("Retrieved {Count} sessions for category {CategoryId}", sessions.Count, categoryId);
                return new PagedResult<Session>
                {
                    Items = sessions,
                    TotalCount = totalCount,
                    CurrentPage = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving sessions for category {CategoryId}", categoryId);
                throw;
            }
        }

        private class SessionResult
        {
            public int TotalCount { get; set; }
        }

        private IQueryable<Session> ApplySorting(IQueryable<Session> query, string sortBy, bool ascending)
        {
            _logger.LogDebug("Applying sorting. SortBy: {SortBy}, Ascending: {Ascending}", sortBy, ascending);
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
                    _logger.LogDebug("Using default sort by StartDate");
                    return ascending ? query.OrderBy(s => s.StartDate) : query.OrderByDescending(s => s.StartDate);
            }
        }
    }
}
