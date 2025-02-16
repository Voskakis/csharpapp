using System.Text;

namespace CSharpApp.Application.Categories
{
    public class CategoriesService : ICategoriesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CategoriesService> _logger;

        public CategoriesService(HttpClient httpClient, ILogger<CategoriesService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<Category> AddCategory(CreateCategory category)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(category), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(string.Empty, jsonContent);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }

        public async Task<IReadOnlyCollection<Category>> GetCategories()
        {
            using var response = await _httpClient.GetAsync(string.Empty);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var res = JsonSerializer.Deserialize<List<Category>>(content);
            return res.AsReadOnly();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            using var response = await _httpClient.GetAsync($"{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Category>(content);
        }
    }
}
