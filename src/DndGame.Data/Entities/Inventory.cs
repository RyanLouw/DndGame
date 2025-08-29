using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Inventory
{
    public Guid CharacterId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
