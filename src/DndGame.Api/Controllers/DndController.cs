using DndGame.Data;
using DndGame.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DndGame.Api.Controllers;

[ApiController]
[Route("api/dnd")]
public class DndController : ControllerBase
{
    private readonly DndGameContext _db;

    public DndController(DndGameContext db)
    {
        _db = db;
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser(string firebaseId, string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == firebaseId);
        if (user == null)
        {
            user = new User
            {
                UserId = firebaseId,
                Email = email,
                CreatedAt = DateTime.UtcNow
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
        }
        var hasCharacter = await _db.Characters.AnyAsync(c => c.UserId == user.UserId);
        var needsCharacterCreation = string.IsNullOrWhiteSpace(user.DisplayName) || !hasCharacter;
        return Ok(new { user, needsCharacterCreation });
    }
}
