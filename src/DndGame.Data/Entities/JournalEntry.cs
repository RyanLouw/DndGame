using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class JournalEntry
{
    public int JournalId { get; set; }

    public Guid CharacterId { get; set; }

    public string EntryText { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Character Character { get; set; } = null!;
}
