
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionsController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<PagedResult<SessionDto>>> GetSessionsByCategory(
        int categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string sortBy = "startDate",
        [FromQuery] bool ascending = true)
    {
        var sessions = await _sessionService.GetSessionsByCategoryAsync(categoryId, page, pageSize, sortBy, ascending);
        return Ok(sessions);
    }
}
