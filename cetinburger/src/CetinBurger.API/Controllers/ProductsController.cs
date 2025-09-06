using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CetinBurger.Application.Contracts.Products;
using CetinBurger.Application.Services;

namespace CetinBurger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
	private readonly IProductService _productService;

	public ProductsController(IProductService productService)
	{
		_productService = productService;
	}
    //GetAll metodu tüm ürünleri getirir.
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
	{
		var products = await _productService.GetAllAsync();
		return Ok(products);
	}

	[HttpGet("{id:int}")]
	[AllowAnonymous]
	public async Task<ActionResult<ProductDto>> GetById([FromRoute] int id)
	{
		var product = await _productService.GetByIdAsync(id);
		if (product is null)
		{
			return NotFound();
		}
		return Ok(product);
	}
    //GetByCategory metodu belirtilen kategoriye ait ürünleri getirir.
	[HttpGet("byCategory/{categoryId:int}")]
	[AllowAnonymous]
	public async Task<ActionResult<IEnumerable<ProductDto>>> GetByCategory([FromRoute] int categoryId)
	{
		try
		{
			var products = await _productService.GetByCategoryAsync(categoryId);
			return Ok(products);
		}
		catch (InvalidOperationException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductRequestDto request)
	{
		try
		{
			var product = await _productService.CreateAsync(request);
			return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
		}
		catch (InvalidOperationException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpPut("{id:int}")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<ProductDto>> Update([FromRoute] int id, [FromBody] UpdateProductRequestDto request)
	{
		try
		{
			var product = await _productService.UpdateAsync(id, request);
			return Ok(product);
		}
		catch (InvalidOperationException ex)
		{
			return NotFound(ex.Message);
		}
	}

	[HttpDelete("{id:int}")]
	[Authorize(Roles = "Admin")]
	public async Task<IActionResult> Delete([FromRoute] int id)
	{
		try
		{
			await _productService.DeleteAsync(id);
			return NoContent();
		}
		catch (InvalidOperationException ex)
		{
			return NotFound(ex.Message);
		}
	}
}


