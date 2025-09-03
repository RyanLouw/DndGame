using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class CharacterClass
{
    public Guid CharacterId { get; set; }

    public Guid ClassId { get; set; }

    public int ClassLevel { get; set; }

    public Guid? SubclassId { get; set; }

    public bool IsPrimary { get; set; }

    public DateTime AddedAt { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Subclass? Subclass { get; set; }
}
