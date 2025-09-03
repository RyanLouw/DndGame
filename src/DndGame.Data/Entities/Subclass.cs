using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Subclass
{
    public Guid SubclassId { get; set; }

    public Guid ClassId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int LevelAvailable { get; set; }

    public virtual ICollection<CharacterClass> CharacterClasses { get; set; } = new List<CharacterClass>();

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<SubclassFeature> SubclassFeatures { get; set; } = new List<SubclassFeature>();
}
