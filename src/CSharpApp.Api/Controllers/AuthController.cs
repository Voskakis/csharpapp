using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CSharpApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/auth/")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly RestApiSettings _restApiSettings;
        private readonly IAuthService _authService;

        public AuthController(ILogger<AuthController> logger, IOptions<RestApiSettings> restApiSettings, IAuthService authService)
        {
            _logger = logger;
            _restApiSettings = restApiSettings.Value;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest requestBody, CancellationToken ct)
        {
            return Ok(await _authService.AuthenticateAsync(requestBody, ct));
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile(CancellationToken ct)
        {
            return null;
        }
    }
}
