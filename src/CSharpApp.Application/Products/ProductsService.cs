using System.Text;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductsService> _logger;
    private readonly RestApiSettings _restApiSettings;

    public ProductsService(ILogger<ProductsService> logger, HttpClient client, IOptions<RestApiSettings> restApiSettings)
    {
        _httpClient = client;
        _logger = logger;
        _restApiSettings = restApiSettings.Value;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts(CancellationToken ct)
    {
        var response = await _httpClient.GetAsync(_restApiSettings.Products, ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(ct);
        var res = JsonSerializer.Deserialize<List<Product>>(content);

        return res.AsReadOnly();
    }

    public async Task<Product> GetProductById(int id, CancellationToken ct)
    {
        using var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{id}", ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<Product>(content);
    }

    public async Task<Product> AddProduct(CreateProduct product, CancellationToken ct)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(_restApiSettings.Products, jsonContent, ct);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(ct);
        return JsonSerializer.Deserialize<Product>(content);
    }
}