using Microsoft.EntityFrameworkCore;
using AutoMapper;

public interface ITagService
{
    Task<IEnumerable<TagDto>> GetAllTagsAsync();
    Task<TagDto> GetTagByIdAsync(int id);
    Task<TagDto> GetTagByNameAsync(string name);
    Task<TagDto> CreateTagAsync(TagDto tagDto);
}

public class TagService : ITagService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TagService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
    {
        var tags = await _context.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();

        return _mapper.Map<IEnumerable<TagDto>>(tags);
    }

    public async Task<TagDto> GetTagByIdAsync(int id)
    {
        var tag = await _context.Tags.FindAsync(id);
        return _mapper.Map<TagDto>(tag);
    }

    public async Task<TagDto> GetTagByNameAsync(string name)
    {
        var tag = await _context.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());

        return _mapper.Map<TagDto>(tag);
    }

    public async Task<TagDto> CreateTagAsync(TagDto tagDto)
    {
        // Check if tag already exists
        var existingTag = await _context.Tags
            .FirstOrDefaultAsync(t => t.Name.ToLower() == tagDto.Name.ToLower());

        if (existingTag != null)
        {
            return _mapper.Map<TagDto>(existingTag);
        }

        // Create new tag
        var tag = _mapper.Map<Tag>(tagDto);
        // tag.CreatedAt = System.DateTime.UtcNow;

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();

        return _mapper.Map<TagDto>(tag);
    }
}
