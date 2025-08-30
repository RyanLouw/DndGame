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
        var user = await _db.Users.Include(u => u.Character)
            .FirstOrDefaultAsync(u => u.UserId == firebaseId);
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
        var needsCharacterCreation = string.IsNullOrWhiteSpace(user.DisplayName) || user.Character == null;
        return Ok(new { user, needsCharacterCreation });
    }

    public class CreateCharacterRequest
    {
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Race { get; set; }
        public string? Alignment { get; set; }
        public string? Backstory { get; set; }
    }

    [HttpPost("character")]
    public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == request.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var character = new Character
        {
            UserId = user.UserId,
            IsActive = true,
            Name = request.Name,
            Race = request.Race,
            Alignment = request.Alignment,
            Backstory = request.Backstory,
            CreatedAt = DateTime.UtcNow
        };

        _db.Characters.Add(character);
        await _db.SaveChangesAsync();

        return Ok(character);
    }
}
