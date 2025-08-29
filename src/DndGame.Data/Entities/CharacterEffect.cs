using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CharacterEffect
{
    public long EffectId { get; set; }

    public Guid CharacterId { get; set; }

    public string Source { get; set; } = null!;

    public DateTime StartedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public int? RoundsTotal { get; set; }

    public int? RoundsLeft { get; set; }

    public bool IsActive { get; set; }

    public string? StackingKey { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual ICollection<CharacterEffectModifier> CharacterEffectModifiers { get; set; } = new List<CharacterEffectModifier>();
}
