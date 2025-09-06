using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CetinBurger.Application.Contracts.Categories;
using CetinBurger.Application.Services;

namespace CetinBurger.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
	private readonly ICategoryService _categoryService;

	public CategoriesController(ICategoryService categoryService)
	{
		_categoryService = categoryService;
	}

    //GetAll metodu tüm kategorileri getirir.
	[HttpGet]
	[AllowAnonymous]
	public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
	{
		var categories = await _categoryService.GetAllAsync();
		return Ok(categories);
	}

	[HttpGet("{id:int}")]
	[AllowAnonymous]
	public async Task<ActionResult<CategoryDto>> GetById([FromRoute] int id)
	{
		var category = await _categoryService.GetByIdAsync(id);
		if (category is null)
		{
			return NotFound();
		}
		return Ok(category);
	}

    //Create metodu yeni bir kategori oluşturur.
	[HttpPost]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryRequestDto request)
	{
		try
		{
			var category = await _categoryService.CreateAsync(request);
			return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
		}
		catch (InvalidOperationException ex)
		{
			return BadRequest(ex.Message);
		}
	}

	[HttpPut("{id:int}")]
	[Authorize(Roles = "Admin")]
	public async Task<ActionResult<CategoryDto>> Update([FromRoute] int id, [FromBody] UpdateCategoryRequestDto request)
	{
		try
		{
			var category = await _categoryService.UpdateAsync(id, request);
			return Ok(category);
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
			await _categoryService.DeleteAsync(id);
			return NoContent();
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(ex.Message);
		}
	}
}


