using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class ClassFeature
{
    public Guid FeatureId { get; set; }

    public Guid ClassId { get; set; }

    public int Level { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsPassive { get; set; }

    public virtual Class Class { get; set; } = null!;
}
