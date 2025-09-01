using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace DndGame.Blazor;

public record FirebaseUser(string Uid, string Email);

public class FirebaseAuthService
{
    private readonly IJSRuntime _js;

    public FirebaseAuthService(IJSRuntime js) => _js = js;

    public ValueTask<FirebaseUser?> SignInWithEmailPassword(string email, string password)
        => _js.InvokeAsync<FirebaseUser?>("firebaseAuth.signIn", email, password);

    public ValueTask<FirebaseUser?> GetCurrentUser()
        => _js.InvokeAsync<FirebaseUser?>("firebaseAuth.getCurrentUser");
}
