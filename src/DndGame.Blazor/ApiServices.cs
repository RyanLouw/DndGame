using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DndGame.Data.Entities; 

namespace DndGame.Blazor
{
    public class ApiServices
    {
        private readonly HttpClient _http;

        public ApiServices(HttpClient http)
        {
            _http = http;
        }

        public async Task<(User? user, bool needsCharacterCreation)> GetUserAsync(string firebaseId, string email)
        {
            var url = $"api/dnd/user?firebaseId={firebaseId}&email={email}";

            var response = await _http.GetFromJsonAsync<UserResponse>(url);

            return (response?.User, response?.NeedsCharacterCreation ?? true);
        }
    }

    // match your API response exactly
    public class UserResponse
    {
        public User? User { get; set; }
        public bool NeedsCharacterCreation { get; set; }
    }
}
