using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class SpellSlot
{
    public Guid CharacterId { get; set; }

    public byte SpellLevel { get; set; }

    public byte SlotsMax { get; set; }

    public byte SlotsCur { get; set; }

    public virtual Character Character { get; set; } = null!;
}
