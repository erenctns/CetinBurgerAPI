using AutoMapper;
using CetinBurger.Application.Contracts.Categories;
using CetinBurger.Domain.Entities;
using CetinBurger.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CetinBurger.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> GetAllAsync()
    {
        var categories = await _dbContext.Categories.AsNoTracking().ToListAsync();
        return _mapper.Map<List<CategoryDto>>(categories);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return category != null ? _mapper.Map<CategoryDto>(category) : null;
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequestDto request)
    {
        var category = _mapper.Map<Category>(request);
        category.CreatedAt = DateTime.UtcNow;

        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryRequestDto request)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            throw new InvalidOperationException($"Category {id} not found.");
        }

        _mapper.Map(request, category);
        category.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<CategoryDto>(category);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _dbContext.Categories.FindAsync(id);
        if (category is null)
        {
            throw new InvalidOperationException($"Category {id} not found.");
        }

        // Kategoriye bağlı ürünler var mı kontrol et
        var hasProducts = await _dbContext.Products.AnyAsync(p => p.CategoryId == id);
        if (hasProducts)
        {
            throw new InvalidOperationException($"Cannot delete category {id} because it has associated products.");
        }

        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }
}
