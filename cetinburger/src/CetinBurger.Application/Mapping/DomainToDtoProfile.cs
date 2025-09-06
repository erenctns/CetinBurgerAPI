using AutoMapper;
using CetinBurger.Application.Contracts.Categories;
using CetinBurger.Application.Contracts.Products;
using CetinBurger.Domain.Entities;
using CetinBurger.Application.Contracts.Auth;
using CetinBurger.Application.Contracts.Carts;
using CetinBurger.Application.Contracts.Orders;
using CetinBurger.Application.Contracts.Users;
using CetinBurger.Infrastructure;

namespace CetinBurger.Application.Mapping;

// Bu profil, Domain entity <-> DTO dönüşümleri için kullanılır.
public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Product, ProductDto>();
        
        // Cart mappings
        CreateMap<CartItem, CartItemDto>()
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product != null ? s.Product.Name : string.Empty))
            .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.UnitPrice * s.Quantity))
            .ForMember(d => d.IsValid, o => o.MapFrom(s => s.Product != null && s.Product.IsAvailable));
        
        CreateMap<Cart, CartDto>()
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Items))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.Items.Sum(i => i.UnitPrice * i.Quantity)));

        // Order mappings
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.LineTotal, o => o.MapFrom(s => s.UnitPrice * s.Quantity));
        
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.Items, o => o.MapFrom(s => s.Items));

        // User mappings
        CreateMap<ApplicationUser, UserDto>()
            .ForMember(d => d.Email, o => o.MapFrom(s => s.Email ?? string.Empty))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName ?? string.Empty))
            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName ?? string.Empty))
            .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName ?? string.Empty))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.PhoneNumber ?? string.Empty));

        // Request -> Entity eşlemeleri (Create/Update)
        CreateMap<CreateCategoryRequestDto, Category>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Description, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Description) ? null : s.Description.Trim()));

        CreateMap<UpdateCategoryRequestDto, Category>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Description, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Description) ? null : s.Description.Trim()));

        CreateMap<CreateProductRequestDto, Product>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Description, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Description) ? null : s.Description.Trim()))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.ImageUrl) ? null : s.ImageUrl.Trim()));

        CreateMap<UpdateProductRequestDto, Product>()
            .ForMember(d => d.Name, o => o.MapFrom(s => s.Name.Trim()))
            .ForMember(d => d.Description, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Description) ? null : s.Description.Trim()))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.ImageUrl) ? null : s.ImageUrl.Trim()));

        // Cart istekleri -> CartItem
        CreateMap<AddCartItemRequestDto, CartItem>()
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));

        CreateMap<UpdateCartItemRequestDto, CartItem>()
            .ForMember(d => d.Quantity, o => o.MapFrom(s => s.Quantity));

        // Checkout request -> Order mapping
        CreateMap<CheckoutRequestDto, Order>()
            .ForMember(d => d.FirstName, o => o.MapFrom(s => s.FirstName.Trim()))
            .ForMember(d => d.LastName, o => o.MapFrom(s => s.LastName.Trim()))
            .ForMember(d => d.DeliveryAddress, o => o.MapFrom(s => s.DeliveryAddress.Trim()))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone.Trim()))
            .ForMember(d => d.Note, o => o.MapFrom(s => string.IsNullOrWhiteSpace(s.Note) ? null : s.Note.Trim()));

        // Admin için: Order + User -> AdminOrderListDto mapping
        CreateMap<Order, AdminOrderListDto>()
            .ForMember(d => d.UserEmail, o => o.Ignore()) // User bilgisi ayrı set edilecek
            .ForMember(d => d.CustomerName, o => o.MapFrom(s => $"{s.FirstName} {s.LastName}".Trim()))
            .ForMember(d => d.DeliveryAddress, o => o.MapFrom(s => s.DeliveryAddress))
            .ForMember(d => d.Phone, o => o.MapFrom(s => s.Phone ?? string.Empty))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status))
            .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalAmount))
            .ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt))
            .ForMember(d => d.ItemCount, o => o.MapFrom(s => s.Items.Count))
            .ForMember(d => d.MailSentStatus, o => o.MapFrom(s => s.MailSentStatus));
    }
}


