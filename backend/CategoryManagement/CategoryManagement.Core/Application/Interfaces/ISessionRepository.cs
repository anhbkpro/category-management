using CategoryManagement.Core.Domain.Entities;

namespace CategoryManagement.Core.Application.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
    {
        Task<PagedResult<Session>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending);
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    }
}
