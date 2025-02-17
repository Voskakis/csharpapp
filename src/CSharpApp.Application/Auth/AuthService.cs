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

        public async Task<AuthResponse> AuthenticateAsync(AuthRequest authRequest, CancellationToken ct)
        {
            using var jsonContent = new StringContent(JsonSerializer.Serialize(authRequest), Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(_restApiSettings.Auth, jsonContent, ct);
            if (!response.IsSuccessStatusCode)
            {
                return new AuthResponse();
            }
            var content = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<AuthResponse>(content) ?? new AuthResponse();
        }

        public async Task<UserProfile> GetUserProfileAsync(string accessToken, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync(_restApiSettings.Profile, ct);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(ct);
            return JsonSerializer.Deserialize<UserProfile>(content);
        }
    }
}
