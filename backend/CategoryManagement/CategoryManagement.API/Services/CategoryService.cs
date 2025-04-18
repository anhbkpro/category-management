using AutoMapper;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> GetCategoryByIdAsync(int id);
    Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto);
    Task UpdateCategoryAsync(CategoryDto categoryDto);
    Task DeleteCategoryAsync(int id);
}

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(IRepository<Category> categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> GetCategoryByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto)
    {
        var category = _mapper.Map<Category>(categoryDto);
        category.CreatedAt = DateTime.UtcNow;

        var result = await _categoryRepository.AddAsync(category);
        return _mapper.Map<CategoryDto>(result);
    }

    public async Task UpdateCategoryAsync(CategoryDto categoryDto)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
        if (existingCategory == null)
            throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found");

        _mapper.Map(categoryDto, existingCategory);
        existingCategory.UpdatedAt = DateTime.UtcNow;

        await _categoryRepository.UpdateAsync(existingCategory);
    }

    public async Task DeleteCategoryAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException($"Category with ID {id} not found");

        await _categoryRepository.DeleteAsync(category);
    }
}

