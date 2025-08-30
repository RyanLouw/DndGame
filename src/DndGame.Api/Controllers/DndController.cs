
using DndGame.Data;
using DndGame.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DndGame.Api.Controllers;

[ApiController]
[Route("api/dnd")]
public class DndController : ControllerBase
{

    private readonly UserDataManager _users;

    public DndController(UserDataManager users)
    {
        _users = users;
    }
    [HttpGet("user")]
    public async Task<IActionResult> GetUser([FromQuery] string? firebaseId, [FromQuery] string? email)
    {
        if (string.IsNullOrWhiteSpace(firebaseId) && string.IsNullOrWhiteSpace(email))
            return BadRequest("Provide either firebaseId or email.");

        var result = await _users.GetUserAsync(firebaseId, email);

        if (result is null)
            return NotFound("User not found.");

        return Ok(result); // if result is a string userId, this returns it; if it's a DTO, it returns the JSON object.
    }
}
