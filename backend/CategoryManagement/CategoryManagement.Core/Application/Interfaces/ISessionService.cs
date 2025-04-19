using CategoryManagement.Core.Application.DTOs;

namespace CategoryManagement.Core.Application.Interfaces
{
    public interface ISessionService
    {
        Task<PagedResult<SessionDto>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending);
    }
}
