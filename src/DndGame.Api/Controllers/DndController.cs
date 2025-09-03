
using DndGame.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Runtime.InteropServices;

namespace DndGame.Api.Controllers;

[ApiController]
[Route("api/dnd")]
public class DndController : ControllerBase
{

    private readonly UserDataAccess _users;
    private readonly CharacterDataAccess _chars;

    public DndController(UserDataAccess users, CharacterDataAccess chars)
    {
        _users = users;
        _chars = chars;
    }
    [HttpGet("user")]
    public async Task<IActionResult> GetUser([FromQuery] string? firebaseId, [FromQuery] string? email)
    {
        if (string.IsNullOrWhiteSpace(firebaseId) && string.IsNullOrWhiteSpace(email))
            return BadRequest("Provide either firebaseId or email.");

        var result = await _users.GetUserAsync(firebaseId, email);

        if (result is null)
            return NotFound("User not found.");

        return Ok(result); 
    }

    // ------------------ CHARACTER: CREATE ------------------
    public record CreateCharacterReq(
        string UserId, string Name, string Race, string Alignment,
        int Str, int Dex, int Con, int Intel, int Wis, int Cha,
        int MaxHPBase, int CurrentHP, int TempHP,
        string? MapId = "start_area", decimal PosX = 0, decimal PosY = 0, string? LastCheckpoint = "start"
    );

    // POST /api/dnd/character
    [HttpPost("character")]
    public async Task<IActionResult> CreateCharacter([FromBody] CreateCharacterReq req, CancellationToken ct)
    {
        var ch = await _chars.CreateCharacterAsync(
            userId: req.UserId, name: req.Name, race: req.Race, alignment: req.Alignment,
            str: req.Str, dex: req.Dex, con: req.Con, intel: req.Intel, wis: req.Wis, cha: req.Cha,
            maxHpBase: req.MaxHPBase, currentHp: req.CurrentHP, tempHp: req.TempHP,
            mapId: req.MapId, posX: req.PosX, posY: req.PosY, lastCheckpoint: req.LastCheckpoint,
            ct: ct);

        return Ok(new { ch.CharacterId, ch.Name, ch.Race, ch.Alignment, ch.IsActive });
    }

    // ------------------ CHARACTER: BASICS ------------------
    public record UpdateBasicsReq(Guid CharacterId, string? Name, string? Race, string? Alignment, bool? IsActive);

    // PATCH /api/dnd/character/basics
    [HttpPatch("character/basics")]
    public async Task<IActionResult> UpdateBasics([FromBody] UpdateBasicsReq req, CancellationToken ct)
    {
        var ch = await _chars.UpdateBasicsAsync(req.CharacterId, req.Name, req.Race, req.Alignment, req.IsActive, ct);
        return Ok(new { ch.CharacterId, ch.Name, ch.Race, ch.Alignment, ch.IsActive });
    }

    // ------------------ CHARACTER: ABILITY SCORES & HP ------------------
    public record UpdateAbilityReq(
        Guid CharacterId, int? Str, int? Dex, int? Con, int? Intel, int? Wis, int? Cha,
        int? MaxHPBase, int? CurrentHP, int? TempHP
    );

    // PATCH /api/dnd/character/abilities
    [HttpPatch("character/abilities")]
    public async Task<IActionResult> UpdateAbilities([FromBody] UpdateAbilityReq req, CancellationToken ct)
    {
        var ab = await _chars.UpdateAbilityScoresAsync(
            req.CharacterId, req.Str, req.Dex, req.Con, req.Intel, req.Wis, req.Cha,
            req.MaxHPBase, req.CurrentHP, req.TempHP, ct);

        return Ok(new
        {
            ab.CharacterId,
            ab.StrengthBase,
            ab.DexterityBase,
            ab.ConstitutionBase,
            ab.IntelligenceBase,
            ab.WisdomBase,
            ab.CharismaBase,
            ab.MaxHpbase,
            ab.CurrentHp,
            ab.TempHp
        });
    }

    // ------------------ INVENTORY ------------------
    public record InventoryChangeReq(int ItemId, int QuantityDelta);
    public record InventoryReq(Guid CharacterId, List<InventoryChangeReq> Changes);

    // POST /api/dnd/character/inventory
    [HttpPost("character/inventory")]
    public async Task<IActionResult> SetInventory([FromBody] InventoryReq req, CancellationToken ct)
    {
        await _chars.SetInventoryAsync(
            req.CharacterId,
            req.Changes.Select(c => (c.ItemId, c.QuantityDelta)),
            ct);
        return NoContent();
    }

    // ------------------ EQUIPMENT ------------------
    public record EquipmentReq(Guid CharacterId, int SlotId, int? ItemId);

    // PUT /api/dnd/character/equipment
    [HttpPut("character/equipment")]
    public async Task<IActionResult> SetEquipment([FromBody] EquipmentReq req, CancellationToken ct)
    {
        await _chars.SetEquipmentAsync(req.CharacterId, req.SlotId, req.ItemId, ct);
        return NoContent();
    }

    // ------------------ SPELLS: KNOWN ------------------
    public record SpellSetReq(Guid CharacterId, List<int> SpellIds);

    // PUT /api/dnd/character/spells/known
    [HttpPut("character/spells/known")]
    public async Task<IActionResult> SetKnownSpells([FromBody] SpellSetReq req, CancellationToken ct)
    {
        await _chars.SetKnownSpellsAsync(req.CharacterId, req.SpellIds, ct);
        return NoContent();
    }

    // ------------------ SPELLS: PREPARED ------------------
    // PUT /api/dnd/character/spells/prepared
    [HttpPut("character/spells/prepared")]
    public async Task<IActionResult> SetPreparedSpells([FromBody] SpellSetReq req, CancellationToken ct)
    {
        await _chars.SetPreparedSpellsAsync(req.CharacterId, req.SpellIds, ct);
        return NoContent();
    }

    // ------------------ SPELL SLOTS ------------------
    public record SpellSlotsLevelReq(int SpellLevel, int SlotsMax, int SlotsCur);
    public record SpellSlotsReq(Guid CharacterId, List<SpellSlotsLevelReq> Levels);

    // PUT /api/dnd/character/spellslots
    [HttpPut("character/spellslots")]
    public async Task<IActionResult> SetSpellSlots([FromBody] SpellSlotsReq req, CancellationToken ct)
    {
        await _chars.SetSpellSlotsAsync(
            req.CharacterId,
            req.Levels.Select(l => (l.SpellLevel, l.SlotsMax, l.SlotsCur)),
            ct);
        return NoContent();
    }

    // ------------------ READ: SNAPSHOT / EFFECTIVE STATS ------------------
    // GET /api/dnd/character/snapshot/{userId}
    [HttpGet("character/snapshot/{userId}")]
    public async Task<IActionResult> GetSnapshot([FromRoute] string userId, CancellationToken ct)
    {
        var snap = await _chars.GetCurrentSnapshotAsync(userId, ct);
        if (snap is null) return NotFound();
        return Ok(snap);
    }

    // GET /api/dnd/character/effective-stats/{characterId}
    [HttpGet("character/effective-stats/{characterId:guid}")]
    public async Task<IActionResult> GetEffectiveStats([FromRoute] Guid characterId, CancellationToken ct)
    {
        var list = await _chars.GetEffectiveStatsAsync(characterId, ct);
        return Ok(list);
    }
}