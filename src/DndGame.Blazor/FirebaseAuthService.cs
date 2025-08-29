using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DndGame.Blazor;

public class FirebaseAuthService
{
    private readonly IJSRuntime _js;

    public FirebaseAuthService(IJSRuntime js) => _js = js;

    public ValueTask<string> SignInWithEmailPassword(string email, string password)
        => _js.InvokeAsync<string>("firebaseAuth.signIn", email, password);

    public ValueTask<string> GetCurrentUser()
        => _js.InvokeAsync<string>("firebaseAuth.getCurrentUser");
}
