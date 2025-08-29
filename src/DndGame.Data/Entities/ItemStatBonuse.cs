using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class ItemStatBonuse
{
    public int ItemId { get; set; }

    public string StatName { get; set; } = null!;

    public short Amount { get; set; }

    public virtual Item Item { get; set; } = null!;
}
