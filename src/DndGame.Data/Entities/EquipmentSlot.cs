using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class EquipmentSlot
{
    public int SlotId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<CharacterEquipment> CharacterEquipments { get; set; } = new List<CharacterEquipment>();
}
