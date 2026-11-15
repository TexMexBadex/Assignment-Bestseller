using NSubstitute;
using Products.DTOs;
using Products.Models;
using Products.Repositories;
using Products.Services;

namespace Products.Tests;

public class ProductServiceUnitTests
{
  private readonly IRepository<Product> _mockRepository;
  private readonly ProductService _productService;

  public ProductServiceUnitTests()
  {
    _mockRepository = Substitute.For<IRepository<Product>>();
    _productService = new ProductService(_mockRepository);
  }

  [Fact]
  public async Task GetProductByIdAsync_WithUnknownId_ReturnsNull()
  {
    // Arrange
    _mockRepository.GetByIdAsync(Arg.Any<int>()).Returns((Product?)null);

    // Act
    var result = await _productService.GetProductByIdAsync(999);

    // Assert
    Assert.Null(result);
  }

  [Fact]
  public async Task CreateProductAsync_WithValidData_SavesAndReturnsProductDto()
  {
    // Arrange
    var createDto = new CreateProductDto
    {
      Title = "Test Product",
      Price = 29.99m,
      Description = "Test description",
      Category = "electronics",
      Image = "https://test.com/img.jpg"
    };

    // Simulate the database assigning an id on insert
    _mockRepository.When(x => x.AddAsync(Arg.Any<Product>()))
      .Do(call => call.Arg<Product>().Id = 42);

    // Act
    var result = await _productService.CreateProductAsync(createDto);

    // Assert
    Assert.Equal(42, result.Id);
    Assert.Equal("Test Product", result.Title);
    Assert.Equal(29.99m, result.Price);
    await _mockRepository.Received(1).AddAsync(Arg.Is<Product>(p => p.Title == "Test Product"));
  }

  [Fact]
  public async Task UpdateProductAsync_WithValidId_UpdatesFieldsAndReturnsTrue()
  {
    // Arrange
    var existing = new Product
    {
      Id = 1,
      Title = "Old",
      Price = 10m,
      Description = "Old desc",
      Category = "old",
      Image = "old-url"
    };
    _mockRepository.GetByIdAsync(1).Returns(existing);

    var dto = new CreateProductDto
    {
      Title = "New",
      Price = 20m,
      Description = "New desc",
      Category = "new",
      Image = "new-url"
    };

    // Act
    var result = await _productService.UpdateProductAsync(1, dto);

    // Assert
    Assert.True(result);
    await _mockRepository.Received(1).UpdateAsync(
      Arg.Is<Product>(p => p.Id == 1 && p.Title == "New" && p.Price == 20m));
  }

  [Fact]
  public async Task UpdateProductAsync_WithUnknownId_ReturnsFalse()
  {
    // Arrange
    _mockRepository.GetByIdAsync(Arg.Any<int>()).Returns((Product?)null);
    var dto = new CreateProductDto { Title = "x", Price = 1m, Description = "x", Category = "x", Image = "x" };

    // Act
    var result = await _productService.UpdateProductAsync(999, dto);

    // Assert
    Assert.False(result);
    await _mockRepository.DidNotReceive().UpdateAsync(Arg.Any<Product>());
  }

  [Fact]
  public async Task DeleteProductAsync_WithValidId_ReturnsTrue()
  {
    // Arrange
    var product = new Product
    {
      Id = 1,
      Title = "To Delete",
      Price = 10m,
      Description = "Desc",
      Category = "Cat",
      Image = "url"
    };
    _mockRepository.GetByIdAsync(1).Returns(product);

    // Act
    var result = await _productService.DeleteProductAsync(1);

    // Assert
    Assert.True(result);
    await _mockRepository.Received(1).DeleteAsync(product);
  }

  [Fact]
  public async Task DeleteProductAsync_WithUnknownId_ReturnsFalse()
  {
    // Arrange
    _mockRepository.GetByIdAsync(Arg.Any<int>()).Returns((Product?)null);

    // Act
    var result = await _productService.DeleteProductAsync(999);

    // Assert
    Assert.False(result);
    await _mockRepository.DidNotReceive().DeleteAsync(Arg.Any<Product>());
  }
}