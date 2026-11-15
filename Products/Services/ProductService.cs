using Products.DTOs;
using Products.Models;
using Products.Repositories;

namespace Products.Services;

public class ProductService(IRepository<Product> repository) : IProductService
{
  public async Task<ProductDto?> GetProductByIdAsync(int id)
  {
    var product = await repository.GetByIdAsync(id);
    return product is null ? null : MapToDto(product);
  }

  public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
  {
    var products = await repository.GetAllAsync();
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

    await repository.AddAsync(product);
    return MapToDto(product);
  }

  public async Task<bool> UpdateProductAsync(int id, CreateProductDto request)
  {
    var product = await repository.GetByIdAsync(id);
    if (product is null)
      return false;

    product.Title = request.Title;
    product.Price = request.Price;
    product.Description = request.Description;
    product.Category = request.Category;
    product.Image = request.Image;

    await repository.UpdateAsync(product);
    return true;
  }

  public async Task<bool> DeleteProductAsync(int id)
  {
    var product = await repository.GetByIdAsync(id);
    if (product is null)
      return false;

    await repository.DeleteAsync(product);
    return true;
  }

  private static ProductDto MapToDto(Product product) => new()
  {
    Id = product.Id,
    Title = product.Title,
    Price = product.Price,
    Description = product.Description,
    Category = product.Category,
    Image = product.Image
  };
}
