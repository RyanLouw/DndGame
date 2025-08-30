namespace DndGame.Domain.Interface;

public interface IUserDataAccess
{
    Task<(string UserId, bool HasCharacter)> GetUserAsync(string firebaseId, string email);
}
