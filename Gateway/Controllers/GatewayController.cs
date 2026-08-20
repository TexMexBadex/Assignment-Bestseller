using Microsoft.AspNetCore.Mvc;

namespace Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatewayController(IHttpClientFactory httpClientFactory) : ControllerBase
{
  private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
  private const string ProductsApiBaseUrl = "https://localhost:7169/api/products";

  [HttpGet]
  public Task<IActionResult> GetAll() => ForwardTo(HttpMethod.Get, ProductsApiBaseUrl);

  [HttpGet("{id}")]
  public Task<IActionResult> GetById(int id) => ForwardTo(HttpMethod.Get, $"{ProductsApiBaseUrl}/{id}");

  [HttpPost]
  public Task<IActionResult> Create([FromBody] object product) => ForwardTo(HttpMethod.Post, ProductsApiBaseUrl, product);

  [HttpPut("{id}")]
  public Task<IActionResult> Update(int id, [FromBody] object product) => ForwardTo(HttpMethod.Put, $"{ProductsApiBaseUrl}/{id}", product);

  [HttpDelete("{id}")]
  public Task<IActionResult> Delete(int id) => ForwardTo(HttpMethod.Delete, $"{ProductsApiBaseUrl}/{id}");

  private async Task<IActionResult> ForwardTo(HttpMethod method, string url, object? body = null)
  {
    var request = new HttpRequestMessage(method, url);

    if (body is not null)
      request.Content = JsonContent.Create(body);


    var response = await _httpClient.SendAsync(request);
    var content = await response.Content.ReadAsStringAsync();

    return Content(content, "application/json");
  }

}
