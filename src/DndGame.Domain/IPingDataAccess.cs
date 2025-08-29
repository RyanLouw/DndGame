namespace DndGame.Domain;

public interface IPingDataAccess
{
    Task<string> GetPingAsync();
}
