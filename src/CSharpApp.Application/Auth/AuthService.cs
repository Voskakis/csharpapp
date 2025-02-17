using System.Text;

namespace CSharpApp.Application.Auth
{
    public class AuthService : IAuthService
    {
        ILogger<AuthService> _logger;
        HttpClient _httpClient;
        RestApiSettings _restApiSettings;

        public AuthService(ILogger<AuthService> logger, HttpClient httpClient, IOptions<RestApiSettings> restApiSettings)
        {
            _logger = logger;
            _httpClient = httpClient;
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<string> GetAccessToken()
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(new
            {
                email = _restApiSettings.Username,
                password = _restApiSettings.Password
            }), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(string.Empty, jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                return string.Empty;
            }
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponse>(content);

            return result?.AccessToken ?? string.Empty;
        }
    }
}
