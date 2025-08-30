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
        if (string.IsNullOrWhiteSpace(firebaseId) || string.IsNullOrWhiteSpace(email))
        {
            return Task.FromResult<string?>(null);
        }

        return _data.GetUserAsync(firebaseId, email);
    }
}

