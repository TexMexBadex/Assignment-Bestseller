using System.Net.Http.Headers;

namespace Gateway.Modules;

public static class GatewayModule
{
  public static IEndpointRouteBuilder MapGatewayEndpoints(this IEndpointRouteBuilder app)
  {
    var products = app.MapGroup("/products");

    products.MapGet("", (HttpContext context, IHttpClientFactory factory) =>
      ForwardAsync(context, factory, HttpMethod.Get, "products"))
      .WithName("GetAllProducts")
      .Produces(200);

    products.MapGet("/{id:int}", (int id, HttpContext context, IHttpClientFactory factory) =>
      ForwardAsync(context, factory, HttpMethod.Get, $"products/{id}"))
      .WithName("GetProductById")
      .Produces(200)
      .Produces(404);

    products.MapPost("", (HttpContext context, IHttpClientFactory factory) =>
      ForwardAsync(context, factory, HttpMethod.Post, "products", copyRequestBody: true))
      .WithName("CreateProduct")
      .Produces(201);

    products.MapPut("/{id:int}", (int id, HttpContext context, IHttpClientFactory factory) =>
      ForwardAsync(context, factory, HttpMethod.Put, $"products/{id}", copyRequestBody: true))
      .WithName("UpdateProduct")
      .Produces(204)
      .Produces(404);

    products.MapDelete("/{id:int}", (int id, HttpContext context, IHttpClientFactory factory) =>
      ForwardAsync(context, factory, HttpMethod.Delete, $"products/{id}"))
      .WithName("DeleteProduct")
      .Produces(204)
      .Produces(404);

    return app;
  }

  private static async Task ForwardAsync(
    HttpContext context,
    IHttpClientFactory factory,
    HttpMethod method,
    string path,
    bool copyRequestBody = false)
  {
    using var request = new HttpRequestMessage(method, path);

    if (copyRequestBody)
    {
      request.Content = new StreamContent(context.Request.Body);
      if (context.Request.ContentType is not null)
        request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(context.Request.ContentType);
    }

    using var response = await factory.CreateClient("ProductsApi").SendAsync(
      request,
      HttpCompletionOption.ResponseHeadersRead,
      context.RequestAborted);

    context.Response.StatusCode = (int)response.StatusCode;

    foreach (var header in response.Headers)
    {
      if (header.Key.Equals("Transfer-Encoding", StringComparison.OrdinalIgnoreCase))
        continue;

      context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    foreach (var header in response.Content.Headers)
      context.Response.Headers[header.Key] = header.Value.ToArray();

    await response.Content.CopyToAsync(context.Response.Body, context.RequestAborted);
  }
}
