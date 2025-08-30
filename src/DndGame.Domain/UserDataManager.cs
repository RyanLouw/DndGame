
using DndGame.Domain.Interface;
namespace DndGame.Domain;

public class UserDataManager
{
    private readonly IUserDataAccess _data;




    public UserDataManager(IUserDataAccess data)
    {
        _data = data;

    }


    public Task<string?> GetUserAsync(string? firebaseId, string? email)
    {
        if (firebaseId == null || email == null)
        {
            return Task.FromResult<string?>(null);
        }
        return _data.GetUserAsync(firebaseId, email)
            .ContinueWith(task => (string?)task.Result, TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
