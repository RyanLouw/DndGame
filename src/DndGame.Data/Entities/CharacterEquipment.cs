using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CharacterEquipment
{
    public Guid CharacterId { get; set; }

    public int SlotId { get; set; }

    public int ItemId { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;

    public virtual EquipmentSlot Slot { get; set; } = null!;
}
