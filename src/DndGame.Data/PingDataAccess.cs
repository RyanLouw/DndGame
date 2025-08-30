using DndGame.Domain.Interface;

namespace DndGame.Data;

public class PingDataAccess : IPingDataAccess
{
    public Task<string> GetPingAsync()
    {
        return Task.FromResult("pong");
    }
}
