using CetinBurger.Application.Contracts.Categories;

namespace CetinBurger.Application.Services;

public interface ICategoryService
{
    Task<List<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request);
    Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequestDto request);
    Task DeleteAsync(int id);
}
