namespace CetinBurger.Application.Contracts.Categories;

// Bu DTO, Category varlıklarının dışarıya (API response) aktarımı için kullanılır.
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}


