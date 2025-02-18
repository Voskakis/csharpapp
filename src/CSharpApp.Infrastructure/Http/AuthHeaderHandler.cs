using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace CSharpApp.Infrastructure.Http
{
    public class AuthHeaderHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthHeaderHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationHeader.Split(" ")[1]);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
