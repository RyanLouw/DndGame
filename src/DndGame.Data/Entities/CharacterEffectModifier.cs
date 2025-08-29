using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CharacterEffectModifier
{
    public long EffectId { get; set; }

    public string StatName { get; set; } = null!;

    public short Amount { get; set; }

    public virtual CharacterEffect Effect { get; set; } = null!;
}
