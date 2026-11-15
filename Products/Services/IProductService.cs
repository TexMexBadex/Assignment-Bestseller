using Products.DTOs;

namespace Products.Services;

public interface IProductService
{
  Task<ProductDto?> GetProductByIdAsync(int id);
  Task<IEnumerable<ProductDto>> GetAllProductsAsync();
  Task<ProductDto> CreateProductAsync(CreateProductDto request);
  Task<bool> UpdateProductAsync(int id, CreateProductDto request);
  Task<bool> DeleteProductAsync(int id);
}
