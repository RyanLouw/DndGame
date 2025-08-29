using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class VCurrentCharacterSnapshot
{
    public string UserId { get; set; } = null!;

    public string? DisplayName { get; set; }

    public Guid CharacterId { get; set; }

    public bool IsActive { get; set; }

    public string CharacterName { get; set; } = null!;

    public string? Race { get; set; }

    public string? Alignment { get; set; }

    public int? Strength { get; set; }

    public int? Dexterity { get; set; }

    public int? Constitution { get; set; }

    public int? Intelligence { get; set; }

    public int? Wisdom { get; set; }

    public int? Charisma { get; set; }

    public int? MaxHp { get; set; }

    public short? CurrentHp { get; set; }

    public short? TempHp { get; set; }

    public string? MapId { get; set; }

    public decimal? PosX { get; set; }

    public decimal? PosY { get; set; }

    public string? LastCheckpoint { get; set; }

    public int? ActiveQuestId { get; set; }

    public string? ActiveQuestName { get; set; }

    public string? NarrativeSummary { get; set; }

    public string? WorldStateJson { get; set; }
}
