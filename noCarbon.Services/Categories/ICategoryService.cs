using noCarbon.Core.Dtos;

namespace noCarbon.Services.Categories;

public interface ICategoryService
{
    Task<IList<CategoryDto>> GetAll();
    Task<CategoryDto> GetById(int id);
    Task Add(CategoryDto dto);
    Task Delete(int id);
    Task Update(CategoryDto dto, int id);
}
