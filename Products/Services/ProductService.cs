using Products.DTOs;
using Products.Models;
using Products.Repositories;

namespace Products.Services;

public class ProductService : IProductService
{
  private readonly IRepository<Product> _repository;

  public ProductService(IRepository<Product> repository)
  {
    _repository = repository;
  }

  public async Task<ProductDto?> GetProductByIdAsync(int id)
  {
    var product = await _repository.GetByIdAsync(id);
    if (product is null)
      return null;

    return MapToDto(product);
  }

  public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
  {
    var products = await _repository.GetAllAsync();
    return products.Select(MapToDto);
  }

  public async Task<ProductDto> CreateProductAsync(CreateProductDto request)
  {
    var product = new Product
    {
      Title = request.Title,
      Price = request.Price,
      Description = request.Description,
      Category = request.Category,
      Image = request.Image
    };

    await _repository.AddAsync(product);
    return MapToDto(product);
  }

  public async Task UpdateProductAsync(int id, CreateProductDto request)
  {
    var product = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with id {id} not found");

    product.Title = request.Title;
    product.Price = request.Price;
    product.Description = request.Description;
    product.Category = request.Category;
    product.Image = request.Image;

    await _repository.UpdateAsync(product);
  }

  public async Task DeleteProductAsync(int id)
  {
    var product = await _repository.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Product with id {id} not found");
    await _repository.DeleteAsync(product);
  }

  private static ProductDto MapToDto(Product product)
  {
    return new ProductDto
    {
      Title = product.Title,
      Price = product.Price,
      Description = product.Description,
      Category = product.Category,
      Image = product.Image
    };
  }
}
