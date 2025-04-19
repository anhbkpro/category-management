using CategoryManagement.Core.Application.DTOs;

namespace CategoryManagement.Core.Application.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<TagDto>> GetAllTagsAsync();
        Task<TagDto> GetTagByIdAsync(int id);
        Task<TagDto> GetTagByNameAsync(string name);
        Task<TagDto> CreateTagAsync(TagDto tagDto);
    }
}
