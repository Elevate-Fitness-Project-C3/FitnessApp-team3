using System.Net.Http.Headers;

namespace NutritionService.Services.FitnessProfile
{
    public sealed class BearerTokenForwardingHandler(IHttpContextAccessor httpContextAccessor):DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authHeader = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrWhiteSpace(authHeader) &&
            AuthenticationHeaderValue.TryParse(authHeader, out var headerValue))
            {
                request.Headers.Authorization = headerValue;
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
