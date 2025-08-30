//using DndGame.Data;
//using DndGame.Data.Entities;
//using DndGame.Domain;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace DndGame.Api.Controllers;

//[ApiController]
//[Route("api")]
//public class TestController : ControllerBase
//{
//    private readonly PingManager _ping;
//    private readonly DndGameContext _db;

//    public TestController(PingManager ping, DndGameContext db)
//    {
//        _ping = ping;
//        _db = db;
//    }

//    [HttpGet("ping")]
//    public async Task<IActionResult> Ping()
//    {
//        var message = await _ping.GetPingAsync();
//        return Ok(new { message });
//    }

//    [HttpGet("users")]
//    public async Task<IActionResult> GetUsers()
//    {
//        var users = await _db.Users.Take(50).ToListAsync();
//        return Ok(users);
//    }

//    [HttpGet("characters")]
//    public async Task<IActionResult> GetCharacters()
//    {
//        var characters = await _db.Characters.Take(50).ToListAsync();
//        return Ok(characters);
//    }

//    [HttpPost("characters")]
//    public async Task<IActionResult> CreateCharacter(CharacterCreateDto dto)
//    {
//        var ch = new Character
//        {
//            UserId = dto.UserId,
//            Name = dto.Name,
//            Race = dto.Race,
//            Alignment = dto.Alignment,
//            IsActive = true,
//            CreatedAt = DateTime.UtcNow
//        };
//        _db.Characters.Add(ch);
//        await _db.SaveChangesAsync();
//        return Created($"/api/characters/{ch.CharacterId}", ch);
//    }

//    public record CharacterCreateDto(string UserId, string Name, string? Race, string? Alignment);
//}
