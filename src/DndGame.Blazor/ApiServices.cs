using DndGame.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DndGame.Blazor
{
    public class ApiServices
    {
        private readonly HttpClient _http;
        private readonly ILogger<ApiServices> _logger;

        public ApiServices(HttpClient http, ILogger<ApiServices> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<(User? user, bool needsCharacterCreation)> GetUserAsync(string firebaseId, string email)
        {
            var url = $"api/dnd/user?firebaseId={firebaseId}&email={email}";
            _logger.LogInformation("Requesting user info from {Url}", url);

            try
            {
                var httpResponse = await _http.GetAsync(url);
                _logger.LogInformation("Received HTTP {StatusCode} from {Url}", httpResponse.StatusCode, url);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    _logger.LogWarning("Request to {Url} failed with status {StatusCode}. Content: {Content}", url, httpResponse.StatusCode, content);
                    httpResponse.EnsureSuccessStatusCode();
                }

                var response = await httpResponse.Content.ReadFromJsonAsync<UserResponse>();
                return (response?.User, response?.NeedsCharacterCreation ?? true);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error fetching user from {Url}", url);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching user from {Url}", url);
                throw;
            }
        }
    }

    // match your API response exactly
    public class UserResponse
    {
        public User? User { get; set; }
        public bool NeedsCharacterCreation { get; set; }
    }
    public class ApiSettings
    {
        public Dictionary<string, string> Apis { get; set; } = new();
    }
}
