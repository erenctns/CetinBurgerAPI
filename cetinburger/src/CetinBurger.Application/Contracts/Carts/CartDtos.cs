namespace CetinBurger.Application.Contracts.Carts;

// Request: Sepete ürün ekleme
public class AddCartItemRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

// Request: Sepet öğesini güncelleme
public class UpdateCartItemRequestDto
{
    public int Quantity { get; set; }
}

// Response: Sepet
public class CartDto
{
    public int Id { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
    public decimal Total { get; set; }
}

// Response: Sepet Öğesi
public class CartItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
    public bool IsValid { get; set; } = true; // Ürün hala geçerli mi?
}

