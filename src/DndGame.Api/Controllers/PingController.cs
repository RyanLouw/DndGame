using DndGame.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DndGame.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PingController : ControllerBase
{
    private readonly PingManager _manager;

    public PingController(PingManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var message = await _manager.GetPingAsync();
        return Ok(new { message });
    }
}
