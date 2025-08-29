using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CharacterState
{
    public Guid CharacterId { get; set; }

    public string? MapId { get; set; }

    public decimal? PosX { get; set; }

    public decimal? PosY { get; set; }

    public string? LastCheckpoint { get; set; }

    public int? ActiveQuestId { get; set; }

    public string? NarrativeSummary { get; set; }

    public string? WorldStateJson { get; set; }

    public virtual Quest? ActiveQuest { get; set; }

    public virtual Character Character { get; set; } = null!;
}
