using AutoMapper;
using CategoryManagement.Core.Application.DTOs;
using CategoryManagement.Core.Application.Interfaces;
using CategoryManagement.Core.Domain;
using CategoryManagement.Core.Domain.Entities;

namespace CategoryManagement.Infrastructure.Services
{
    public class CategoryConditionService : ICategoryConditionService
    {
        private readonly ICategoryConditionRepository _categoryConditionRepository;
        private readonly IMapper _mapper;

        public CategoryConditionService(ICategoryConditionRepository categoryConditionRepository, IMapper mapper)
        {
            _categoryConditionRepository = categoryConditionRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryConditionDto>> GetConditionsByCategoryIdAsync(int categoryId)
        {
            var conditions = await _categoryConditionRepository.GetByCategoryIdAsync(categoryId);
            return _mapper.Map<IEnumerable<CategoryConditionDto>>(conditions);
        }

        public async Task<CategoryConditionDto> GetCategoryConditionByIdAsync(int id)
        {
            var condition = await _categoryConditionRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryConditionDto>(condition);
        }

        public async Task<CategoryConditionDto> CreateCategoryConditionAsync(CategoryConditionDto conditionDto)
        {
            var condition = _mapper.Map<CategoryCondition>(conditionDto);
            await _categoryConditionRepository.AddAsync(condition);
            return _mapper.Map<CategoryConditionDto>(condition);
        }

        public async Task<CategoryConditionDto> UpdateCategoryConditionAsync(CategoryConditionDto conditionDto)
        {
            var condition = _mapper.Map<CategoryCondition>(conditionDto);
            await _categoryConditionRepository.UpdateAsync(condition);
            return _mapper.Map<CategoryConditionDto>(condition);
        }

        public async Task DeleteCategoryConditionAsync(int id)
        {
            var condition = await _categoryConditionRepository.GetByIdAsync(id);
            if (condition != null)
            {
                await _categoryConditionRepository.DeleteAsync(condition);
            }
        }
    }
}
