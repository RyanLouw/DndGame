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
            try
            {
                var response = await _http.GetFromJsonAsync<UserResponse>(url);
                return (response?.User, response?.NeedsCharacterCreation ?? true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user from {Url}", url);
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
