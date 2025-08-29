using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal? Weight { get; set; }

    public decimal? ValueGp { get; set; }

    public string? Properties { get; set; }

    public virtual ICollection<CharacterEquipment> CharacterEquipments { get; set; } = new List<CharacterEquipment>();

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<ItemStatBonuse> ItemStatBonuses { get; set; } = new List<ItemStatBonuse>();
}
