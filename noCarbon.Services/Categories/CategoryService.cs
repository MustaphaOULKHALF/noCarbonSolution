using noCarbon.Core.Configurations;
using noCarbon.Core.Domain;
using noCarbon.Core.Dtos;
using noCarbon.Data;

namespace noCarbon.Services.Categories;

public partial class CategoryService : ICategoryService
{
    #region Fields
    private readonly AppSettings _appSettings;
    private readonly IRepository<Category> _categoryRepository;
    #endregion
    #region Ctor
    public CategoryService(AppSettings appSettings,
        IRepository<Category> categoryRepository)
    {
        this._appSettings = appSettings;
        this._categoryRepository = categoryRepository;
    }
    #endregion
    #region Methods
    public async Task<IList<CategoryDto>> GetAll()
    {
        var categories = _categoryRepository.TableNoTracking;
        return await categories.Select(c => new CategoryDto(c.Id) { Name = c.Name, Description = c.Description, Icon = c.Icon }).ToListAsync();
    }
    public async Task<CategoryDto> GetById(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return new CategoryDto(category.Id) { Name = category.Name, Description = category.Description, Icon = category.Icon };
    }
    public async Task Add(CategoryDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        await _categoryRepository.InsertAsync(new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            Icon = dto.Icon,
        });
    }

    public async Task Delete(int id)
    {
        if (id == 0)
            throw new ArgumentNullException(nameof(id));
        var category = await _categoryRepository.GetByIdAsync(id);
        _categoryRepository.Delete(category);
    }

    public async Task Update(CategoryDto dto, int id)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        var category = await _categoryRepository.GetByIdAsync(id);
        if (category == null)
            throw new KeyNotFoundException();
        category.Name = dto.Name;
        category.Description = dto.Description;
        category.Icon = dto.Icon;
        _categoryRepository.Update(category);
    }
    #endregion

}
