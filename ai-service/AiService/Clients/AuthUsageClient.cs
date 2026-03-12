using AiService.Interfaces.Clients;
using AiService.Options;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AiService.Clients
{
    public class AuthUsageClient : IAuthUsageClient
    {
        private readonly HttpClient _httpClient;
        private readonly AuthServiceOptions _options;
        private readonly ILogger<AuthUsageClient> _logger;

        public AuthUsageClient(
            HttpClient httpClient,
            IOptions<AuthServiceOptions> options,
            ILogger<AuthUsageClient> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<(bool Allowed, int RemainingCalls, string Message)> CheckUsageAsync(string bearerToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_options.BaseUrl}/api/users/usage");
            request.Headers.Authorization = BuildAuthHeader(bearerToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Usage check failed with status code {StatusCode}", response.StatusCode);
                return (false, 0, "Unable to verify API usage.");
            }

            var result = await response.Content.ReadFromJsonAsync<UsageCheckResponse>();
            if (result == null)
            {
                return (false, 0, "Invalid usage response.");
            }

            return (result.RemainingCalls > 0, result.RemainingCalls, result.Message ?? string.Empty);
        }

        public async Task<(bool Success, int RemainingCalls, string Message)> DecrementUsageAsync(string bearerToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_options.BaseUrl}/api/users/decrement-usage");
            request.Headers.Authorization = BuildAuthHeader(bearerToken);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Usage decrement failed with status code {StatusCode}", response.StatusCode);
                return (false, 0, "Unable to update API usage.");
            }

            var result = await response.Content.ReadFromJsonAsync<UsageCheckResponse>();
            if (result == null)
            {
                return (false, 0, "Invalid usage update response.");
            }

            return (true, result.RemainingCalls, result.Message ?? string.Empty);
        }

        private static AuthenticationHeaderValue BuildAuthHeader(string bearerToken)
        {
            var token = bearerToken.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase).Trim();
            return new AuthenticationHeaderValue("Bearer", token);
        }

        private sealed class UsageCheckResponse
        {
            public int RemainingCalls { get; set; }
            public string? Message { get; set; }
        }
    }
}