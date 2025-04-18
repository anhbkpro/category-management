
using AutoMapper;

public interface ISessionService
{
    Task<PagedResult<SessionDto>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending);
}

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    public SessionService(ISessionRepository sessionRepository, IMapper mapper)
    {
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<SessionDto>> GetSessionsByCategoryAsync(int categoryId, int page, int pageSize, string sortBy, bool ascending)
    {
        var pagedResult = await _sessionRepository.GetSessionsByCategoryAsync(categoryId, page, pageSize, sortBy, ascending);

        return new PagedResult<SessionDto>
        {
            Items = _mapper.Map<IEnumerable<SessionDto>>(pagedResult.Items),
            TotalCount = pagedResult.TotalCount,
            CurrentPage = pagedResult.CurrentPage,
            PageSize = pagedResult.PageSize
        };
    }
}
