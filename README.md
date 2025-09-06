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
cetinburger/
├── src/
│   ├── CetinBurger.API/           # API Katmanı (Controllers, Program.cs)
│   ├── CetinBurger.Application/  # Uygulama Katmanı (Services, DTOs, Validation)
│   ├── CetinBurger.Domain/        # Domain Katmanı (Entities)
│   └── CetinBurger.Infrastructure/ # Altyapı Katmanı (DbContext, Migrations)
├── .gitignore                     # Git ignore dosyası
└── README.md                      # Proje dokümantasyonu

```

**Clean Architecture Katmanları:**
- **API:** HTTP istekleri, Controllers, Middleware
- **Application:** Business logic, Services, DTOs, Validation
- **Domain:** Entities, Business rules
- **Infrastructure:** Database, External services

# 📚 API ENDPOINTS IN SWAGGER UI
<img width="1156" height="885" alt="Image" src="https://github.com/user-attachments/assets/2bcb69fa-7fb2-41c2-8180-6e2f004ef386" />
<img width="1099" height="727" alt="Image" src="https://github.com/user-attachments/assets/c5f8ec7a-b49e-4b92-abed-065f112db827" />

# 📚 API ENDPOINTS

### 🔐 Authentication
- `POST /api/auth/register` - Kullanıcı kaydı
- `POST /api/auth/login` - Kullanıcı girişi

### 🍔 Products
- `GET /api/products` - Ürün listesi
- `GET /api/products/{id}` - Ürün detayı
- `GET /api/products/byCategory/{categoryId}` - Kategoriye göre ürünler
- `POST /api/products` - Ürün ekleme (Admin)
- `PUT /api/products/{id}` - Ürün güncelleme (Admin)
- `DELETE /api/products/{id}` - Ürün silme (Admin)

### 🛒 Cart
- `GET /api/cart` - Sepet görüntüleme
- `POST /api/cart/items` - Sepete ürün ekleme
- `PUT /api/cart/items/{itemId}` - Sepet ürünü güncelleme
- `DELETE /api/cart/items/{itemId}` - Sepetten ürün silme

### 📦 Orders
- `POST /api/orders/checkout` - Sipariş verme
- `GET /api/orders/my` - Siparişlerimi görüntüleme
- `GET /api/orders/{id}` - Sipariş detayı
- `GET /api/orders/admin/all` - Tüm siparişler (Admin)
- `PUT /api/orders/{id}/mailStatus` - Mail gönderim durumu güncelleme (Admin)
- `DELETE /api/orders/{id}` - Sipariş silme (Admin)

### 🏷️ Categories
- `GET /api/categories` - Kategori listesi
- `GET /api/categories/{id}` - Kategori detayı
- `POST /api/categories` - Kategori ekleme (Admin)
- `PUT /api/categories/{id}` - Kategori güncelleme (Admin)
- `DELETE /api/categories/{id}` - Kategori silme (Admin)

### 👥 Users (Admin Only)
- `GET /api/users` - Kullanıcı listesi (Sayfalı)
- `GET /api/users/{id}` - Kullanıcı detayı
- `PUT /api/users/{id}/role` - Kullanıcı rolü güncelleme
- `DELETE /api/users/{id}` - Kullanıcı silme

### 💳 Payment
- `POST /api/payment/process` - Ödeme işlemi
- `GET /api/payment/status/{paymentIntentId}` - Ödeme durumu
- `POST /api/payment/test` - Test endpoint'i

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
POST /api/payment/process
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
Projede **N8N** local olarak kurulmuş ve ödeme alındıktan sonra ilgili kullanıcıya sipariş bilgilerini içeren **otomatik email gönderimi** için kullanılmıştır fakat uyguladıktan sonra projeden kaldırılmıştır.

**N8N'in Amacı:**
- ✅ Ödeme başarılı olduktan sonra **otomatik email** gönderimi
- ✅ Sipariş detaylarını içeren **yapay zeka ile oluşturulmuş kişiselleştirilmiş email**
- ✅ **Local workflow** ile ödeme durumu takibi
- ✅ **Workflow automation** ile süreç otomasyonu

### 🛒 ÖRNEK SENARYO  🛒###
** Kullanıcı girişi , Sepete ürün ekleme , Sepetteki ürünü onaylama , Ödeme kısmı

** Stripe ödeme adımı öncesindeki mevcut tutar( TEST DASHBOARD )

** Stripe ödeme adımı sonrasındaki mevcut tutar( TEST DASHBOARD )

** N8N entegrasyonu ile status:"Paid" ve mailSentStatus:"False" olan Orderları alıp ilgili kullanıcıya AI Agent 'ın oluşturduğu sipariş mesajını mail atıyoruz ve mail sonrası eğer mail olumluysa mailSentStatus'u :"True" olarak set ediyoruz.

---


