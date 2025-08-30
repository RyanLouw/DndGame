using DndGame.Domain.Interface;

namespace DndGame.Domain;

public class PingManager
{
    private readonly IPingDataAccess _data;

    public PingManager(IPingDataAccess data)
    {
        _data = data;
    }

    public Task<string> GetPingAsync()
    {
        return _data.GetPingAsync();
    }
}
