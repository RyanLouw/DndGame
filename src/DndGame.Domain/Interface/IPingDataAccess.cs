namespace DndGame.Domain.Interface;

public interface IPingDataAccess
{
    Task<string> GetPingAsync();
}
