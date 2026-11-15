using Products.DTOs;
using Products.Models;

namespace Products.Services;

public interface IProductService
{
  Task<ProductDto?> GetProductByIdAsync(int id);
  Task<IEnumerable<ProductDto>> GetAllProductsAsync();
  Task<ProductDto> CreateProductAsync(CreateProductDto request);
  Task UpdateProductAsync(int id, CreateProductDto request);
  Task DeleteProductAsync(int id);
}
