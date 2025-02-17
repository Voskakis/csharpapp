namespace CSharpApp.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> AuthenticateAsync(AuthRequest authRequest, CancellationToken ct);
        Task<UserProfile> GetUserProfileAsync(string accessToken, CancellationToken ct);
    }
}
