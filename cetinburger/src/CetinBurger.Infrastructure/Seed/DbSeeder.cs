using CetinBurger.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CetinBurger.Infrastructure.Seed;

public static class DbSeeder
{
	public static async Task SeedAsync(IServiceProvider services)
	{
		using var scope = services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();//user için 
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(); //role için 

		// Optional: ensure database is created/migrated
		await context.Database.MigrateAsync();

		// Seed roles, roller oluşturuluyor.
		string[] roles = ["Admin", "Customer"];
		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new IdentityRole(role));
			}
		}

		// Seed default admin , usermanager ile 
		var adminEmail = "admin@cetinburger.com";
		var admin = await userManager.FindByEmailAsync(adminEmail);
		if (admin is null)
		{
			admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
			var result = await userManager.CreateAsync(admin, "Cetinburger123.");
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(admin, "Admin");
			}
		}
	}
}


