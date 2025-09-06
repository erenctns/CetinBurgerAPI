using Serilog;
using CetinBurger.Infrastructure;
using CetinBurger.Infrastructure.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using FluentValidation.AspNetCore;
using CetinBurger.API.Filters;

var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, services, loggerConfiguration) =>
{
	loggerConfiguration
		.ReadFrom.Configuration(context.Configuration)
		.Enrich.FromLogContext()
		.WriteTo.Console()
		.WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day);
});

// Services
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAutoMapper(typeof(CetinBurger.Application.Mapping.DomainToDtoProfile).Assembly);

// Application Services
builder.Services.AddScoped<CetinBurger.Application.Services.IProductService, CetinBurger.Application.Services.ProductService>();
builder.Services.AddScoped<CetinBurger.Application.Services.ICategoryService, CetinBurger.Application.Services.CategoryService>();
builder.Services.AddScoped<CetinBurger.Application.Services.ICartService, CetinBurger.Application.Services.CartService>();
builder.Services.AddScoped<CetinBurger.Application.Services.IOrderService, CetinBurger.Application.Services.OrderService>();
builder.Services.AddScoped<CetinBurger.Application.Services.IAuthService, CetinBurger.Application.Services.AuthService>();
builder.Services.AddScoped<CetinBurger.Application.Services.IUserService, CetinBurger.Application.Services.UserService>();
builder.Services.AddScoped<CetinBurger.Application.Services.IPaymentService, CetinBurger.Application.Services.StripePaymentService>();


builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationActionFilter>();
});

// FluentValidation: modern registration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CetinBurger.Application.Validation.Categories.CreateCategoryRequestDtoValidator>(); // bura sadece işaret amaçlı
//bu oraya işaret ederek tüm validation işlemlerini otomatik yapar.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CetinBurger.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste token only (no 'Bearer ')."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] { }
        }
    });
});
builder.Services.AddAuthorization();

// JWT Auth - appsettings.json'dan al
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var secret = builder.Configuration["Jwt:SecretKey"];
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = true,
		ValidateIssuerSigningKey = true,
		ValidIssuer = issuer,
		ValidAudience = audience,
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret!))
	};
});

var app = builder.Build();

// Global exception -> ProblemDetails
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var problem = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://httpstatuses.com/500"
        };
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(problem);
    });
});

// Pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.InjectJavascript("/swagger-ui/logout.js");
	});
}

app.UseStaticFiles();
app.UseAuthentication();
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseAuthorization();
app.MapControllers();

// Seed database on startup , authomatic migration
await DbSeeder.SeedAsync(app.Services);

app.Run();
