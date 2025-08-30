namespace DndGame.Domain.Interface;

public interface IUserDataAccess
{
    Task<string> GetUserAsync(string firebaseId, string email);
}
