using System.Text;

namespace CSharpApp.Application.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriesService> _logger;
        private readonly RestApiSettings _restApiSettings;

        public CategoriesService(HttpClient httpClient, ILogger<CategoriesService> logger, IOptions<RestApiSettings> restApiSettings)
        {
            _httpClient = httpClient;
            _logger = logger;
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<Category> AddCategory(CreateCategory category, CancellationToken ct)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(_restApiSettings.Categories, jsonContent, ct);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task<IReadOnlyCollection<Category>> GetCategories(CancellationToken ct)
        {
            using var response = await _httpClient.GetAsync(_restApiSettings.Categories, ct);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(ct);
            var res = JsonSerializer.Deserialize<List<Category>>(content);
            return res.AsReadOnly();
        }

        public async Task<Category> GetCategoryById(int id, CancellationToken ct)
        {
            using var response = await _httpClient.GetAsync($"{_restApiSettings.Categories}/{id}", ct);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<Category>(content);
        }
    }
}
