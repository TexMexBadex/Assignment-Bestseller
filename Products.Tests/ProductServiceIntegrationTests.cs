using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.DTOs;
using Products.Models;
using Products.Repositories;
using Products.Services;

namespace Products.Tests;

public class ProductServiceIntegrationTests : IAsyncLifetime
{
  private readonly IDbContextFactory<AppDbContext> _contextFactory;
  private readonly IProductService _productService;

  public ProductServiceIntegrationTests()
  {
    var dbName = $"ProductsDb_Test_{Guid.NewGuid()}";
    var options = new DbContextOptionsBuilder<AppDbContext>()
      .UseInMemoryDatabase(dbName)
      .Options;

    _contextFactory = new TestDbContextFactory(options);
    var repository = new Repository<Product>(_contextFactory);
    _productService = new ProductService(repository);
  }

  async Task IAsyncLifetime.InitializeAsync()
  {
    await using var context = await _contextFactory.CreateDbContextAsync();

    context.Products.Add(new Product
    {
      Id = 1,
      Title = "Test Product",
      Price = 50m,
      Description = "Desc",
      Category = "electronics",
      Image = "https://test.com/img.jpg"
    });

    await context.SaveChangesAsync();
  }

  Task IAsyncLifetime.DisposeAsync() => Task.CompletedTask;

  [Fact]
  public async Task GetProductByIdAsync_WithValidId_ReturnsDtoWithCorrectData()
  {
    // Act
    var result = await _productService.GetProductByIdAsync(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal("Test Product", result.Title);
    Assert.Equal(50m, result.Price);
  }

  [Fact]
  public async Task GetAllProductsAsync_ReturnsAllProductsAsDtos()
  {
    // Act
    var results = await _productService.GetAllProductsAsync();

    // Assert
    Assert.Single(results);
  }

  [Fact]
  public async Task CreateProductAsync_WithValidDto_InsertsAndReturnsMappedDto()
  {
    // Arrange
    var createDto = new CreateProductDto
    {
      Title = "New Product",
      Price = 99.99m,
      Description = "New desc",
      Category = "jewelery",
      Image = "https://test.com/img2.jpg"
    };

    // Act
    var result = await _productService.CreateProductAsync(createDto);
    var allProducts = await _productService.GetAllProductsAsync();

    // Assert
    Assert.NotNull(result);
    Assert.True(result.Id > 0);
    Assert.Equal("New Product", result.Title);
    Assert.Equal(2, allProducts.Count());
  }

  [Fact]
  public async Task UpdateProductAsync_WithValidData_UpdatesAndReturnsTrue()
  {
    // Arrange
    var updateDto = new CreateProductDto
    {
      Title = "Updated Title",
      Price = 75m,
      Description = "Updated desc",
      Category = "electronics",
      Image = "https://test.com/img.jpg"
    };

    // Act
    var result = await _productService.UpdateProductAsync(1, updateDto);
    var updated = await _productService.GetProductByIdAsync(1);

    // Assert
    Assert.True(result);
    Assert.NotNull(updated);
    Assert.Equal("Updated Title", updated.Title);
    Assert.Equal(75m, updated.Price);
  }

  [Fact]
  public async Task DeleteProductAsync_WithValidId_DeletesAndReturnsTrue()
  {
    // Act
    var result = await _productService.DeleteProductAsync(1);
    var deleted = await _productService.GetProductByIdAsync(1);

    // Assert
    Assert.True(result);
    Assert.Null(deleted);
  }
}