using System;
using System.Collections.Generic;

namespace DndGame.Data.Entities;

public partial class Proficiency
{
    public Guid ProficiencyId { get; set; }

    public string Type { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
}
