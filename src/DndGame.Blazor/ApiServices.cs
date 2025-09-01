using DndGame.Data.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DndGame.Blazor
{
    public class ApiServices
    {
        private readonly HttpClient _http;

        public ApiServices(HttpClient http)
        {
            _http = http;
        }

        public Task<User?> GetUserAsync(string firebaseId, string email)
        {
            var url = $"api/dnd/user?firebaseId={firebaseId}&email={email}";
            return _http.GetFromJsonAsync<User>(url);
        }
    }

    public class ApiSettings
    {
        public Dictionary<string, string> Apis { get; set; } = new();
    }
}
