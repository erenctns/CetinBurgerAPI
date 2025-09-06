using CetinBurger.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CetinBurger.Infrastructure;
//identity kısmı
public class ApplicationUser : IdentityUser
{
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Address { get; set; } = string.Empty;
}

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
	}

	public DbSet<Category> Categories { get; set; } = null!;
	public DbSet<Product> Products { get; set; } = null!;
	public DbSet<Cart> Carts { get; set; } = null!;
	public DbSet<CartItem> CartItems { get; set; } = null!;
	public DbSet<Order> Orders { get; set; } = null!;
	public DbSet<OrderItem> OrderItems { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Category>(entity =>
		{
			entity.ToTable("Categories");
			entity.Property(p => p.Name).HasMaxLength(100).IsRequired();
			entity.Property(p => p.Description).HasMaxLength(500);
		});

		modelBuilder.Entity<Product>(entity =>
		{
			entity.ToTable("Products");
			entity.Property(p => p.Name).HasMaxLength(150).IsRequired();
			entity.Property(p => p.Description).HasMaxLength(1000);
			entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
			entity.HasOne(p => p.Category)
				.WithMany(c => c.Products)
				.HasForeignKey(p => p.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);
		});

		modelBuilder.Entity<Cart>(entity =>
		{
			entity.ToTable("Carts");
			entity.Property(c => c.UserId).HasMaxLength(450).IsRequired();
			entity.HasIndex(c => c.UserId).IsUnique();
		});

		modelBuilder.Entity<CartItem>(entity =>
		{
			entity.ToTable("CartItems");
			entity.Property(ci => ci.UnitPrice).HasColumnType("decimal(18,2)");
			entity.Property(ci => ci.Quantity).IsRequired();
			entity.HasOne(ci => ci.Cart)
				.WithMany(c => c.Items)
				.HasForeignKey(ci => ci.CartId)
				.OnDelete(DeleteBehavior.Cascade);
			entity.HasOne(ci => ci.Product)
				.WithMany()
				.HasForeignKey(ci => ci.ProductId)
				.OnDelete(DeleteBehavior.Restrict);
			entity.HasIndex(ci => new { ci.CartId, ci.ProductId }).IsUnique();
		});

		modelBuilder.Entity<Order>(entity =>
		{
			entity.ToTable("Orders");
			entity.Property(o => o.UserId).HasMaxLength(450).IsRequired();
			entity.Property(o => o.Status).HasMaxLength(50).IsRequired();
			entity.Property(o => o.TotalAmount).HasColumnType("decimal(18,2)");
			entity.Property(o => o.FirstName).HasMaxLength(50).IsRequired();
			entity.Property(o => o.LastName).HasMaxLength(50).IsRequired();
			entity.Property(o => o.DeliveryAddress).HasMaxLength(200).IsRequired();
			entity.Property(o => o.Phone).HasMaxLength(20);
			entity.Property(o => o.Note).HasMaxLength(500);
		});

		modelBuilder.Entity<OrderItem>(entity =>
		{
			entity.ToTable("OrderItems");
			entity.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
			entity.Property(oi => oi.ProductName).HasMaxLength(200).IsRequired();
			entity.HasOne(oi => oi.Order)
				.WithMany(o => o.Items)
				.HasForeignKey(oi => oi.OrderId)
				.OnDelete(DeleteBehavior.Cascade);
			entity.HasOne<Product>()
				.WithMany()
				.HasForeignKey(oi => oi.ProductId)
				.OnDelete(DeleteBehavior.SetNull);
		});
	}
}
