using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

namespace CetinBurger.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		// appsettings.json'dan connection string al
		var connectionString = configuration.GetConnectionString("DefaultConnection");
		services.AddDbContext<AppDbContext>(options =>
			options.UseSqlServer(connectionString));

		services
			.AddIdentityCore<ApplicationUser>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 8;
			})
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<AppDbContext>();



		return services;
	}
}
//Bu kısmı çok fazla anlamadım , sonradan tekrar bakacağım.
