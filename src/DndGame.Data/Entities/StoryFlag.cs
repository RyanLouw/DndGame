using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class StoryFlag
{
    public Guid CharacterId { get; set; }

    public string FlagKey { get; set; } = null!;

    public string? FlagValue { get; set; }

    public virtual Character Character { get; set; } = null!;
}
