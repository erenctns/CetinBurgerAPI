using CetinBurger.Application.Contracts.Products;

namespace CetinBurger.Application.Services;

public interface IProductService
{
    //PRODUCT CONTROLLER'I İÇİNDEKİ METHODLARIN HEPSİNİ BURAYA YAZDI , ProductService'i implemente ederek kullanırız.
    Task<List<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<List<ProductDto>> GetByCategoryAsync(int categoryId);
    Task<ProductDto> CreateAsync(CreateProductRequestDto request);
    Task<ProductDto> UpdateAsync(int id, UpdateProductRequestDto request);
    Task DeleteAsync(int id);
}
