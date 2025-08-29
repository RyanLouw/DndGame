using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CharacterQuest
{
    public Guid CharacterId { get; set; }

    public int QuestId { get; set; }

    public string Status { get; set; } = null!;

    public DateTime UpdatedAt { get; set; }

    public virtual Character Character { get; set; } = null!;

    public virtual Quest Quest { get; set; } = null!;
}
