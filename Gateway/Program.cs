using Gateway.Middleware;
using Gateway.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient("ProductsApi", client =>
{
  client.BaseAddress = new Uri("https://localhost:7169/");
  client.DefaultRequestHeaders.Add("X-Gateway-Key", builder.Configuration["GatewayKey"]);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<TimerMiddleware>();
app.MapGatewayEndpoints();

app.Run();
