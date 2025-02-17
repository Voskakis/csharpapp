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

        public async Task<Category> AddCategory(CreateCategory category)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(_restApiSettings.Categories, jsonContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task<IReadOnlyCollection<Category>> GetCategories()
        {
            using var response = await _httpClient.GetAsync(_restApiSettings.Categories);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<Category>>(content);
            return res.AsReadOnly();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            using var response = await _httpClient.GetAsync($"{_restApiSettings.Categories}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }
    }
}
