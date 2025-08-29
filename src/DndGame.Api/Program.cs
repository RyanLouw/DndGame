using DndGame.Data;               // namespace from scaffolded project
using DndGame.Data.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<DndGameContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS (allow local dev UI later)
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(p => p
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowed(_ => true) // dev only
    );
});

var app = builder.Build();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI();

// Health
app.MapGet("/", () => "DND Game API OK");

// --- Minimal test endpoints ---
app.MapGet("/api/users", async (DndGameContext db) =>
    await db.Users.Take(50).ToListAsync());

app.MapGet("/api/characters", async (DndGameContext db) =>
    await db.Characters.Take(50).ToListAsync());

// Seed (dev only)
app.MapPost("/dev/seed", async (DndGameContext db) =>
{
    if (await db.Users.FindAsync("test-uid") is null)
    {
        db.Users.Add(new User { UserId = "test-uid", Email = "test@example.com", CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();
    }
    if (!await db.Characters.AnyAsync(c => c.UserId == "test-uid"))
    {
        db.Characters.Add(new Character { UserId = "test-uid", Name = "Aerin", Race = "Elf", Alignment = "CG", IsActive = true, CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();
    }
    return Results.Ok(new { ok = true });
});

// Create character (basic)
app.MapPost("/api/characters", async (DndGameContext db, CharacterCreateDto dto) =>
{
    var ch = new Character
    {
        UserId = dto.UserId,  // later: derive from Firebase token
        Name = dto.Name,
        Race = dto.Race,
        Alignment = dto.Alignment,
        IsActive = true,
        CreatedAt = DateTime.UtcNow
    };
    db.Characters.Add(ch);
    await db.SaveChangesAsync();
    return Results.Created($"/api/characters/{ch.CharacterId}", ch);
});

app.Run();

public record CharacterCreateDto(string UserId, string Name, string? Race, string? Alignment);
