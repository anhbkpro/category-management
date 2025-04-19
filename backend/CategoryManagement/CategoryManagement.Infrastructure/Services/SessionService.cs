using AutoMapper;
using CategoryManagement.Core.Application.DTOs;
using CategoryManagement.Core.Application.Interfaces;

namespace CategoryManagement.Infrastructure.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public SessionService(ISessionRepository sessionRepository, IMapper mapper)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }

        public async Task<PagedResult<SessionDto>> GetSessionsByCategoryAsync(
            int categoryId,
            int page = 1,
            int pageSize = 10,
            string sortBy = "StartDate",
            bool ascending = true)
        {
            var result = await _sessionRepository.GetSessionsByCategoryAsync(
                categoryId,
                page,
                pageSize,
                sortBy,
                ascending);

            return new PagedResult<SessionDto>
            {
                Items = _mapper.Map<System.Collections.Generic.List<SessionDto>>(result.Items),
                TotalCount = result.TotalCount,
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize
            };
        }
    }
}
