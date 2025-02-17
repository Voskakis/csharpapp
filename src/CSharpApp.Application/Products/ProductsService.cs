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

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        var response = await _httpClient.GetAsync(_restApiSettings.Products);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var res = JsonSerializer.Deserialize<List<Product>>(content);

        return res.AsReadOnly();
    }

    public async Task<Product> GetProductById(int id)
    {
        using var response = await _httpClient.GetAsync($"{_restApiSettings.Products}/{id}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Product>(content);
    }

    public async Task<Product> AddProduct(CreateProduct product)
    {
        using var jsonContent = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
        using var response = await _httpClient.PostAsync(_restApiSettings.Products, jsonContent);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Product>(content);
    }
}