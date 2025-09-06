namespace CetinBurger.Application.Contracts.Auth;

// Bu DTO, API'de kullanıcı kayıt (register) isteği için kullanılır.
// Zorunlu alanlar: FirstName, LastName, Email, Password, Address, PhoneNumber
// PhoneNumber: 0 ile başlayan 11 hane (örn. 05555555555). Gönderim farklı formatta olsa da
//              controller katmanında normalize edilir.
public class RegisterRequestDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}


