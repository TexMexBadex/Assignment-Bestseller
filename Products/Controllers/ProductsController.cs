using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext db) : ControllerBase
{
  private readonly AppDbContext _db = db;

  [HttpGet]
  public async Task<IActionResult> GetAll()
  {
    var products = await _db.Products.ToListAsync();

    return Ok(products);
  }

  [HttpGet("{id}")]
  public async Task<IActionResult> GetById(int id)
  {
    var product = await _db.Products.FindAsync(id);

    return product is not null ? Ok(product) : NotFound();
  }

  [HttpPost]
  public async Task<IActionResult> Create([FromBody] Product request)
  {
    if (request == null)
    {
      return BadRequest();
    }

    _db.Products.Add(request);
    await _db.SaveChangesAsync();

    return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(int id, Product request)
  {
    if (id != request.Id)
    {
      return BadRequest("Id in URL does not match id in body");
    }

    var existing = await _db.Products.FindAsync(id);

    if (existing is null)
      return NotFound();

    existing.Title = request.Title;
    existing.Price = request.Price;
    existing.Description = request.Description;
    existing.Category = request.Category;
    existing.Image = request.Image;

    await _db.SaveChangesAsync();

    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(int id)
  {
    var product = await _db.Products.FindAsync(id);

    if (product is null)
      return NotFound();

    _db.Products.Remove(product);

    await _db.SaveChangesAsync();

    return NoContent();
  }
}
