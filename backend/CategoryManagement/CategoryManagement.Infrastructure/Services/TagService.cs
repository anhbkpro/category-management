using AutoMapper;
using CategoryManagement.Core.Application.DTOs;
using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain.Entities;

namespace CategoryManagement.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;
        private readonly IMapper _mapper;

        public TagService(IRepository<Tag> tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TagDto>> GetAllTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<TagDto> GetTagByIdAsync(int id)
        {
            var tag = await _tagRepository.GetByIdAsync(id);
            return _mapper.Map<TagDto>(tag);
        }

        public async Task<TagDto> GetTagByNameAsync(string name)
        {
            // Note: This method requires a custom repository implementation
            // You might want to create an ITagRepository interface with this specific method
            throw new System.NotImplementedException();
        }

        public async Task<TagDto> CreateTagAsync(TagDto tagDto)
        {
            var tag = _mapper.Map<Tag>(tagDto);
            await _tagRepository.AddAsync(tag);
            return _mapper.Map<TagDto>(tag);
        }
    }
}
