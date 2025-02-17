namespace CSharpApp.Core.Dtos
{
    public class AuthRequest
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }


        [JsonPropertyName("password")]
        public string? Password { get; set; }
    }
}
