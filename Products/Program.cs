using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.Models;
using Products.Modules;
using Products.Repositories;
using Products.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContextFactory<AppDbContext>(options => options.UseInMemoryDatabase("ProductsDb"));
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
  var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
  await using var db = await factory.CreateDbContextAsync();
  DbSeeder.Seed(db);
}

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapProductsEndpoints();

//Check if the request has the correct gateway key in the headers to make sure 
// API is called only through the gateway
app.Use(async (context, next) =>
{
  var expected = app.Configuration["GatewayKey"];

  if (string.IsNullOrEmpty(expected) ||
      context.Request.Headers["X-Gateway-Key"] != expected)
  {
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    return;
  }

  await next();
});

app.Run();
