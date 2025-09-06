namespace CetinBurger.Domain.Entities;

//SEPETTEKİ ÜRÜNLERİN HER BİRİNİN BİLGİLERİ
public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; } // hangi sepete ait oldığu
    public int ProductId { get; set; } // hangi ürünün sepete gireceği.
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Cart? Cart { get; set; } // cart numarası atıyorum 3, git bana cart id'si 3 olan cartın bilgilerini getir.
    public Product? Product { get; set; } // product numarası atıyorum 5, git bana product id'si 5 olan productın bilgilerini getir. (ilgili id noya sahip elemanı getirir)
}


