using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class StoryEvent
{
    public long EventId { get; set; }

    public Guid CharacterId { get; set; }

    public string EventType { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Character Character { get; set; } = null!;
}
