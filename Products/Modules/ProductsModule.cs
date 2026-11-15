using Products.DTOs;
using Products.Services;

namespace Products.Modules;

public static class ProductsModule
{
  public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
  {
    var products = app.MapGroup("/products");

    products.MapGet("", async (IProductService service) =>
      Results.Ok(await service.GetAllProductsAsync()));

    products.MapGet("/{id:int}", async (int id, IProductService service) =>
    {
      var product = await service.GetProductByIdAsync(id);
      return product is null ? Results.NotFound() : Results.Ok(product);
    });

    products.MapPost("", async (CreateProductDto request, IProductService service) =>
    {
      var product = await service.CreateProductAsync(request);
      return Results.Created($"/products/{product.Id}", product);
    });

    products.MapPut("/{id:int}", async (int id, CreateProductDto request, IProductService service) =>
    {
      var updated = await service.UpdateProductAsync(id, request);
      return updated ? Results.NoContent() : Results.NotFound();
    });

    products.MapDelete("/{id:int}", async (int id, IProductService service) =>
    {
      var deleted = await service.DeleteProductAsync(id);
      return deleted ? Results.NoContent() : Results.NotFound();
    });

    return app;
  }
}
