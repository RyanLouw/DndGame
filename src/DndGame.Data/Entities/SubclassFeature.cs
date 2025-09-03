using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class SubclassFeature
{
    public Guid FeatureId { get; set; }

    public Guid SubclassId { get; set; }

    public int Level { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPassive { get; set; }

    public virtual Subclass Subclass { get; set; } = null!;
}
