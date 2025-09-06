# CetinBurger E-COMMERCE API

# 👋 HERKESE MERHABA!

Bu projenin asıl amacı **yapay zeka araçlarını efektif bir şekilde kullanarak** Clean Architecture kurallarına uygun bir **E-Ticaret API'si** geliştirmek, ayrıca **Stripe ödeme entegrasyonu** bağlayarak demo olarak gerçek bir ödeme sistemi kullanmak ve **N8N entegrasyonu** ile kullanıcıya otomatik mail göndermektir.

---

Clean Architecture ile geliştirilmiş burger restoranı **E-Ticaret API'si**.

# 🚀 ÖZELLİKLER

- ✅ **.NET 8.0** 
- ✅ **Clean Architecture** (Domain, Application, Infrastructure, API)
- ✅ **ASP.NET Core Identity** (Kullanıcı yönetimi)
- ✅ **JWT Authentication** (Token tabanlı kimlik doğrulama)
- ✅ **Entity Framework Core** (MSSQL veritabanı)
- ✅ **AutoMapper** (Object mapping)
- ✅ **FluentValidation** (Request validation)
- ✅ **Stripe Payment Integration** (Ödeme sistemi)
- ✅ **N8N Integration** (Workflow automation ve email gönderimi)
- ✅ **Swagger/OpenAPI** (API dokümantasyonu)

# 📋 GEREKSİNİMLER

- .NET 8.0
- SQL Server
- Stripe Hesabı (Test için)

# 📋 IDE
- Cursor

# 🔧 KURULUM

### 1. Repository'yi Klonlayın
git clone https://github.com/yourusername/CetinBurger.git
cd CetinBurger

### 2. Veritabanı Bağlantısı
- SQL Server'ınızı çalıştırın
- Connection string'i `appsettings.Development.json`'da güncelleyin

### 3. Konfigürasyon Dosyasını Oluşturun

copy appsettings.example.json appsettings.Development.json

### 4. API Anahtarlarını Ayarlayın
`appsettings.Development.json` dosyasını düzenleyin:

```json
{
  "Jwt": {
    "Issuer": "CetinBurger",
    "Audience": "CetinBurger",
    "SecretKey": "YOUR_JWT_SECRET_KEY_HERE_MINIMUM_32_CHARACTERS_LONG"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=CetinBurgerDb;Integrated Security=True;TrustServerCertificate=True;"
  },
  "Stripe": {
    "SecretKey": "sk_test_YOUR_STRIPE_SECRET_KEY_HERE"
  }
}
```


### 5. Migration'ları Çalıştırın
dotnet ef database update --project src/CetinBurger.Infrastructure

### 6. API'yi Çalıştırın
dotnet run --project src/CetinBurger.API

**Not:** API çalıştıktan sonra terminal'de Swagger linki görünecek, oradan API dokümantasyonuna erişebilirsiniz.


### ⚠️ Önemli Notlar:
- `.env` dosyasındaki bilgilerinizi `appsettings.Development.json`'a koyup oradan kullanabilirsiniz
# 📁 PROJE YAPISI

```
CetinBurger/
├── src/
│   ├── CetinBurger.API/           # API Katmanı (Controllers, Program.cs)
│   ├── CetinBurger.Application/  # Uygulama Katmanı (Services, DTOs, Validation)
│   ├── CetinBurger.Domain/        # Domain Katmanı (Entities)
│   └── CetinBurger.Infrastructure/ # Altyapı Katmanı (DbContext, Migrations)
├── appsettings.example.json       # Örnek konfigürasyon dosyası
├── .gitignore                     # Git ignore dosyası
└── README.md                      # Proje dokümantasyonu
```

**Clean Architecture Katmanları:**
- **API:** HTTP istekleri, Controllers, Middleware
- **Application:** Business logic, Services, DTOs, Validation
- **Domain:** Entities, Business rules
- **Infrastructure:** Database, External services

# 📚 API ENDPOINTS

### 🔐 Authentication
- `POST /api/Auth/register` - Kullanıcı kaydı
- `POST /api/Auth/login` - Kullanıcı girişi

### 🍔 Products
- `GET /api/Products` - Ürün listesi
- `GET /api/Products/{id}` - Ürün detayı
- `GET /api/Products/byCategory/{categoryId}` - Kategoriye göre ürünler
- `POST /api/Products` - Ürün ekleme (Admin)
- `PUT /api/Products/{id}` - Ürün güncelleme (Admin)
- `DELETE /api/Products/{id}` - Ürün silme (Admin)

### 🛒 Cart
- `GET /api/Cart` - Sepet görüntüleme
- `POST /api/Cart/items` - Sepete ürün ekleme
- `PUT /api/Cart/items/{itemId}` - Sepet ürünü güncelleme
- `DELETE /api/Cart/items/{itemId}` - Sepetten ürün silme

### 📦 Orders
- `POST /api/Orders/checkout` - Sipariş verme
- `GET /api/Orders/my` - Siparişlerimi görüntüleme
- `GET /api/Orders/{id}` - Sipariş detayı
- `GET /api/Orders/admin/all` - Tüm siparişler (Admin)
- `PUT /api/Orders/{id}/mailStatus` - Mail gönderim durumu güncelleme (Admin)
- `DELETE /api/Orders/{id}` - Sipariş silme (Admin)

### 🏷️ Categories
- `GET /api/Categories` - Kategori listesi
- `GET /api/Categories/{id}` - Kategori detayı
- `POST /api/Categories` - Kategori ekleme (Admin)
- `PUT /api/Categories/{id}` - Kategori güncelleme (Admin)
- `DELETE /api/Categories/{id}` - Kategori silme (Admin)

### 👥 Users (Admin Only)
- `GET /api/Users` - Kullanıcı listesi (Sayfalı)
- `GET /api/Users/{id}` - Kullanıcı detayı
- `PUT /api/Users/{id}/role` - Kullanıcı rolü güncelleme
- `DELETE /api/Users/{id}` - Kullanıcı silme

### 💳 Payment
- `POST /api/Payment/process` - Ödeme işlemi
- `GET /api/Payment/status/{paymentIntentId}` - Ödeme durumu
- `POST /api/Payment/test` - Test endpoint'i

# 🧪 TEST

### Test Kullanıcıları:
Migration çalıştırdıktan sonra otomatik olarak oluşturulan test kullanıcıları:

**Admin Kullanıcısı:**
- Email: SeedDatadan bakabilirsiniz
- Şifre: SeedDatadan bakabilirsiniz
- Rol: Admin


### Test Kartları:
- **Başarılı:** 4242 4242 4242 4242
- **Başarısız:** 4000 0000 0000 0002
- **Yetersiz Bakiye:** 4000 0000 0000 9995

### Örnek Ödeme:
```json
POST /api/Payment/process
{
  "amount": 9000,
  "currency": "try",
  "paymentMethodId": "",
  "description": "Test ödeme",
  "orderId": 1,
  "customerEmail": "test@example.com"
}
```

**Not:** `amount` değeri **kuruş** cinsinden yazılır. Örnek: `9000` = **90 TL**

**Not:** `paymentMethodId` boş bırakılabilir çünkü sistem otomatik olarak Stripe'ın test kartı (`tok_visa`) ile örnek bir PaymentMethod oluşturur.

# 🔄 N8N WORKFLOW AUTOMATION

### 📧 Email Automation
Projede **N8N** local olarak kurulmuş ve ödeme alındıktan sonra ilgili kullanıcıya sipariş bilgilerini içeren **otomatik email gönderimi** için kullanılmıştır.

**N8N'in Amacı:**
- ✅ Ödeme başarılı olduktan sonra **otomatik email** gönderimi
- ✅ Sipariş detaylarını içeren **yapay zeka ile oluşturulmuş kişiselleştirilmiş email**
- ✅ **Local workflow** ile ödeme durumu takibi
- ✅ **Workflow automation** ile süreç otomasyonu



---

Bu proje eğitim amaçlıdır. Production'da kullanmadan önce güvenlik önlemlerini alın.
