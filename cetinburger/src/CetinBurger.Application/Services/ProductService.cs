using AutoMapper;
using CetinBurger.Application.Contracts.Products;
using CetinBurger.Domain.Entities;
using CetinBurger.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CetinBurger.Application.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext; // dbcontext için
    private readonly IMapper _mapper; // automapper için

    public ProductService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _dbContext.Products.AsNoTracking().ToListAsync();
        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<List<ProductDto>> GetByCategoryAsync(int categoryId)
    {
        var categoryExists = await _dbContext.Categories.AnyAsync(c => c.Id == categoryId);
        if (!categoryExists)
        {
            throw new InvalidOperationException($"Category {categoryId} not found.");
        }

        var products = await _dbContext.Products
            .Where(p => p.CategoryId == categoryId)
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequestDto request)
    {
        var existsCategory = await _dbContext.Categories.AnyAsync(c => c.Id == request.CategoryId);
        if (!existsCategory)
        {
            throw new InvalidOperationException($"Category {request.CategoryId} not found.");
        }

        var product = _mapper.Map<Product>(request);
        product.CreatedAt = DateTime.UtcNow;

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> UpdateAsync(int id, UpdateProductRequestDto request)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product {id} not found.");
        }

        if (!await _dbContext.Categories.AnyAsync(c => c.Id == request.CategoryId))
        {
            throw new InvalidOperationException($"Category {request.CategoryId} not found.");
        }

        _mapper.Map(request, product);
        product.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _dbContext.Products.FindAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product {id} not found.");
        }

        // Ürünün sepette kullanılıp kullanılmadığını kontrol et
        var cartItems = await _dbContext.CartItems
            .Where(ci => ci.ProductId == id)
            .ToListAsync();

        if (cartItems.Any())
        {
            // Sepetten bu ürünleri kaldır
            _dbContext.CartItems.RemoveRange(cartItems);
        }

        // Ürünü sil (OrderItem'larda ProductId nullable olduğu için sorun olmaz)
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
    }
}
